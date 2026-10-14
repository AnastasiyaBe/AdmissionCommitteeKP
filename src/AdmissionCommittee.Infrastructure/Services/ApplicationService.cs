using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Domain.Enums;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AdmissionCommittee.Infrastructure.Services;

using Application = AdmissionCommittee.Domain.Entities.Application;
public class ApplicationService : IApplicationService
{
    private readonly AdmissionCommitteeDbContext _db;

    public ApplicationService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<ApplicationDto>> GetAllAsync()
    {
        return await _db.Applications
            .AsNoTracking()
            .Include(x => x.Applicant)
            .Include(x => x.Specialty)
            .OrderByDescending(x => x.SubmissionDateTime)
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
    }

    public async Task<ApplicationDto?> GetByIdAsync(int id)
    {
        var x = await _db.Applications
            .AsNoTracking()
            .Include(a => a.Applicant)
            .Include(a => a.Specialty)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);

        if (x is null) return null;

        return new ApplicationDto
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
        };
    }

    public async Task<int> CreateAsync(ApplicationDto dto)
    {
        var entity = new Application
        {
            ApplicationNumber = dto.ApplicationNumber,
            ApplicantId = dto.ApplicantId,
            SpecialtyId = dto.SpecialtyId,
            Priority = dto.Priority,
            SubmissionDateTime = dto.SubmissionDateTime == default
                ? DateTime.Now
                : dto.SubmissionDateTime,
            Status = Enum.Parse<ApplicationStatus>(dto.Status)
        };

        _db.Applications.Add(entity);
        await _db.SaveChangesAsync();
        return entity.ApplicationId;
    }

    public async Task UpdateAsync(ApplicationDto dto)
    {
        var entity = await _db.Applications.FindAsync(dto.ApplicationId)
            ?? throw new InvalidOperationException($"Application {dto.ApplicationId} не найдено");

        entity.ApplicationNumber = dto.ApplicationNumber;
        entity.ApplicantId = dto.ApplicantId;
        entity.SpecialtyId = dto.SpecialtyId;
        entity.Priority = dto.Priority;
        entity.SubmissionDateTime = dto.SubmissionDateTime;
        entity.Status = Enum.Parse<ApplicationStatus>(dto.Status);

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Applications.FindAsync(id);
        if (entity is null) return;

        _db.Applications.Remove(entity);
        await _db.SaveChangesAsync();
    }
}