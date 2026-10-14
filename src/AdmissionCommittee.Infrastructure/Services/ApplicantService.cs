using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class ApplicantService : IApplicantService
{
    private readonly AdmissionCommitteeDbContext _db;

    public ApplicantService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<ApplicantDto>> GetAllAsync()
    {
        return await _db.Applicants
            .AsNoTracking()
            .OrderBy(a => a.FullName)
            .Select(a => ToDto(a))
            .ToListAsync();
    }

    public async Task<ApplicantDto?> GetByIdAsync(int id)
    {
        var a = await _db.Applicants.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantId == id);
        return a is null ? null : ToDto(a);
    }

    public async Task<int> CreateAsync(ApplicantDto dto)
    {
        var entity = new Applicant
        {
            FullName = dto.FullName,
            BirthDate = dto.BirthDate,
            Passport = dto.Passport,
            Address = dto.Address,
            Phone = dto.Phone,
            EducationDocument = dto.EducationDocument,
            AverageScore = dto.AverageScore,
            Benefits = dto.Benefits
        };

        _db.Applicants.Add(entity);
        await _db.SaveChangesAsync();
        return entity.ApplicantId;
    }

    public async Task UpdateAsync(ApplicantDto dto)
    {
        var entity = await _db.Applicants.FindAsync(dto.ApplicantId)
            ?? throw new InvalidOperationException($"Applicant {dto.ApplicantId} не найден");

        entity.FullName = dto.FullName;
        entity.BirthDate = dto.BirthDate;
        entity.Passport = dto.Passport;
        entity.Address = dto.Address;
        entity.Phone = dto.Phone;
        entity.EducationDocument = dto.EducationDocument;
        entity.AverageScore = dto.AverageScore;
        entity.Benefits = dto.Benefits;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Applicants.FindAsync(id);
        if (entity is null) return;

        _db.Applicants.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<ApplicantSearchDto?> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return null;

        var q = query.Trim();

        // Поиск по фамилии (фрагмент FullName) или по номеру заявления
        var applicant = await _db.Applicants
            .AsNoTracking()
            .FirstOrDefaultAsync(a =>
                a.FullName.Contains(q) ||
                a.Applications.Any(ap => ap.ApplicationNumber == q));

        if (applicant is null) return null;

        var applications = await _db.Applications
            .AsNoTracking()
            .Include(x => x.Specialty)
            .Include(x => x.Applicant)
            .Where(x => x.ApplicantId == applicant.ApplicantId)
            .OrderBy(x => x.Priority)
            .Select(x => new ApplicationDto
            {
                ApplicationId = x.ApplicationId,
                ApplicationNumber = x.ApplicationNumber,
                ApplicantId = x.ApplicantId,
                ApplicantFullName = x.Applicant.FullName,
                SpecialtyId = x.SpecialtyId,
                SpecialtyName = x.Specialty.Name,
                Priority = x.Priority,
                SubmissionDateTime = x.SubmissionDateTime,
                Status = x.Status.ToString()
            })
            .ToListAsync();

        var testResults = await _db.TestResults
            .AsNoTracking()
            .Include(x => x.Test)
            .Include(x => x.Applicant)
            .Where(x => x.ApplicantId == applicant.ApplicantId)
            .Select(x => new TestResultDto
            {
                ResultId = x.ResultId,
                ApplicantId = x.ApplicantId,
                ApplicantFullName = x.Applicant.FullName,
                TestId = x.TestId,
                TestName = x.Test.Name,
                Score = x.Score,
                ResultDate = x.ResultDate,
                CertificateNumber = x.CertificateNumber
            })
            .ToListAsync();

        return new ApplicantSearchDto
        {
            Applicant = ToDto(applicant),
            Applications = applications,
            TestResults = testResults
        };
    }

    private static ApplicantDto ToDto(Applicant a) => new()
    {
        ApplicantId = a.ApplicantId,
        FullName = a.FullName,
        BirthDate = a.BirthDate,
        Passport = a.Passport,
        Address = a.Address,
        Phone = a.Phone,
        EducationDocument = a.EducationDocument,
        AverageScore = a.AverageScore,
        Benefits = a.Benefits
    };
}