using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly AdmissionCommitteeDbContext _db;

    public SpecialtyService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<SpecialtyDto>> GetAllAsync()
    {
        return await _db.Specialties
            .AsNoTracking()
            .Include(s => s.Faculty)
            .OrderBy(s => s.Code)
            .Select(s => new SpecialtyDto
            {
                SpecialtyId = s.SpecialtyId,
                Code = s.Code,
                Name = s.Name,
                FacultyId = s.FacultyId,
                FacultyName = s.Faculty.Name,
                EducationForm = s.EducationForm.ToString(),
                DurationYears = s.DurationYears,
                BudgetPlan = s.BudgetPlan,
                PaidPlan = s.PaidPlan
            })
            .ToListAsync();
    }

    public async Task<SpecialtyDto?> GetByIdAsync(int id)
    {
        var s = await _db.Specialties
            .AsNoTracking()
            .Include(x => x.Faculty)
            .FirstOrDefaultAsync(x => x.SpecialtyId == id);

        if (s is null) return null;

        return new SpecialtyDto
        {
            SpecialtyId = s.SpecialtyId,
            Code = s.Code,
            Name = s.Name,
            FacultyId = s.FacultyId,
            FacultyName = s.Faculty.Name,
            EducationForm = s.EducationForm.ToString(),
            DurationYears = s.DurationYears,
            BudgetPlan = s.BudgetPlan,
            PaidPlan = s.PaidPlan
        };
    }

    public async Task<int> CreateAsync(SpecialtyDto dto)
    {
        var entity = new Specialty
        {
            Code = dto.Code,
            Name = dto.Name,
            FacultyId = dto.FacultyId,
            EducationForm = Enum.Parse<Domain.Enums.EducationForm>(dto.EducationForm),
            DurationYears = dto.DurationYears,
            BudgetPlan = dto.BudgetPlan,
            PaidPlan = dto.PaidPlan
        };

        _db.Specialties.Add(entity);
        await _db.SaveChangesAsync();
        return entity.SpecialtyId;
    }

    public async Task UpdateAsync(SpecialtyDto dto)
    {
        var entity = await _db.Specialties.FindAsync(dto.SpecialtyId)
            ?? throw new InvalidOperationException($"Specialty {dto.SpecialtyId} не найдена");

        entity.Code = dto.Code;
        entity.Name = dto.Name;
        entity.FacultyId = dto.FacultyId;
        entity.EducationForm = Enum.Parse<Domain.Enums.EducationForm>(dto.EducationForm);
        entity.DurationYears = dto.DurationYears;
        entity.BudgetPlan = dto.BudgetPlan;
        entity.PaidPlan = dto.PaidPlan;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Specialties.FindAsync(id);
        if (entity is null) return;

        _db.Specialties.Remove(entity);
        await _db.SaveChangesAsync();
    }
}