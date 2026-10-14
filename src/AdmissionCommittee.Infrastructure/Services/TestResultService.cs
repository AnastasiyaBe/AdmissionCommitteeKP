using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class TestResultService : ITestResultService
{
    private readonly AdmissionCommitteeDbContext _db;

    public TestResultService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<TestResultDto>> GetAllAsync()
    {
        return await _db.TestResults
            .AsNoTracking()
            .Include(x => x.Applicant)
            .Include(x => x.Test)
            .OrderByDescending(x => x.ResultDate)
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
    }

    public async Task<TestResultDto?> GetByIdAsync(int id)
    {
        var x = await _db.TestResults
            .AsNoTracking()
            .Include(a => a.Applicant)
            .Include(a => a.Test)
            .FirstOrDefaultAsync(a => a.ResultId == id);

        if (x is null) return null;

        return new TestResultDto
        {
            ResultId = x.ResultId,
            ApplicantId = x.ApplicantId,
            ApplicantFullName = x.Applicant.FullName,
            TestId = x.TestId,
            TestName = x.Test.Name,
            Score = x.Score,
            ResultDate = x.ResultDate,
            CertificateNumber = x.CertificateNumber
        };
    }

    public async Task<int> CreateAsync(TestResultDto dto)
    {
        var entity = new TestResult
        {
            ApplicantId = dto.ApplicantId,
            TestId = dto.TestId,
            Score = dto.Score,
            ResultDate = dto.ResultDate,
            CertificateNumber = dto.CertificateNumber
        };

        _db.TestResults.Add(entity);
        await _db.SaveChangesAsync();
        return entity.ResultId;
    }

    public async Task UpdateAsync(TestResultDto dto)
    {
        var entity = await _db.TestResults.FindAsync(dto.ResultId)
            ?? throw new InvalidOperationException($"TestResult {dto.ResultId} не найден");

        entity.ApplicantId = dto.ApplicantId;
        entity.TestId = dto.TestId;
        entity.Score = dto.Score;
        entity.ResultDate = dto.ResultDate;
        entity.CertificateNumber = dto.CertificateNumber;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.TestResults.FindAsync(id);
        if (entity is null) return;

        _db.TestResults.Remove(entity);
        await _db.SaveChangesAsync();
    }
}