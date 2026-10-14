using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Domain.Enums;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class EntranceTestService : IEntranceTestService
{
    private readonly AdmissionCommitteeDbContext _db;

    public EntranceTestService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<EntranceTestDto>> GetAllAsync()
    {
        return await _db.EntranceTests
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new EntranceTestDto
            {
                TestId = t.TestId,
                Name = t.Name,
                Form = t.Form.ToString(),
                MinScore = t.MinScore
            })
            .ToListAsync();
    }

    public async Task<EntranceTestDto?> GetByIdAsync(int id)
    {
        var t = await _db.EntranceTests.AsNoTracking().FirstOrDefaultAsync(x => x.TestId == id);
        if (t is null) return null;

        return new EntranceTestDto
        {
            TestId = t.TestId,
            Name = t.Name,
            Form = t.Form.ToString(),
            MinScore = t.MinScore
        };
    }

    public async Task<int> CreateAsync(EntranceTestDto dto)
    {
        var entity = new EntranceTest
        {
            Name = dto.Name,
            Form = Enum.Parse<TestForm>(dto.Form),
            MinScore = dto.MinScore
        };

        _db.EntranceTests.Add(entity);
        await _db.SaveChangesAsync();
        return entity.TestId;
    }

    public async Task UpdateAsync(EntranceTestDto dto)
    {
        var entity = await _db.EntranceTests.FindAsync(dto.TestId)
            ?? throw new InvalidOperationException($"EntranceTest {dto.TestId} не найдено");

        entity.Name = dto.Name;
        entity.Form = Enum.Parse<TestForm>(dto.Form);
        entity.MinScore = dto.MinScore;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.EntranceTests.FindAsync(id);
        if (entity is null) return;

        _db.EntranceTests.Remove(entity);
        await _db.SaveChangesAsync();
    }
}