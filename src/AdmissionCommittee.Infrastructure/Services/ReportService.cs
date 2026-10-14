using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Enums;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly AdmissionCommitteeDbContext _db;

    public ReportService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Конкурсный список по специальности: сумма баллов, сортировка по убыванию, отображение статуса.
    /// </summary>
    public async Task<List<CompetitiveListItemDto>> GetCompetitiveListAsync(int specialtyId, string? fundingForm = null)
    {
        var applications = await _db.Applications
            .AsNoTracking()
            .Include(a => a.Applicant)
            .Include(a => a.Specialty)
            .Where(a => a.SpecialtyId == specialtyId)
            .ToListAsync();

        if (applications.Count == 0) return new List<CompetitiveListItemDto>();

        var applicantIds = applications.Select(a => a.ApplicantId).Distinct().ToList();
        var testResults = await _db.TestResults
            .AsNoTracking()
            .Where(r => applicantIds.Contains(r.ApplicantId))
            .ToListAsync();

        var testsByApplicant = testResults
            .GroupBy(r => r.ApplicantId)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.Score));

        var list = applications.Select(a => new CompetitiveListItemDto
        {
            ApplicationId = a.ApplicationId,
            ApplicationNumber = a.ApplicationNumber,
            ApplicantId = a.ApplicantId,
            ApplicantFullName = a.Applicant.FullName,
            AverageScore = a.Applicant.AverageScore,
            TestsScore = testsByApplicant.TryGetValue(a.ApplicantId, out var s) ? s : 0m,
            TotalScore = a.Applicant.AverageScore + (testsByApplicant.TryGetValue(a.ApplicantId, out var s2) ? s2 : 0m),
            Priority = a.Priority,
            Status = a.Status.ToString()
        })
        .OrderByDescending(x => x.TotalScore)
        .ThenBy(x => x.Priority)
        .ToList();

        return list;
    }

    /// <summary>
    /// Текущий проходной балл: минимальный TotalScore среди зачисленных (или среди тех, кто проходит по плану).
    /// Простейшая реализация: берём первые N по конкурсному списку, где N = сумма мест на специальности,
    /// и возвращаем TotalScore последнего.
    /// </summary>
    public async Task<decimal> GetCurrentPassingScoreAsync(int specialtyId)
    {
        var specialty = await _db.Specialties
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SpecialtyId == specialtyId);

        if (specialty is null) return 0m;

        var plan = specialty.BudgetPlan + specialty.PaidPlan;

        var list = await GetCompetitiveListAsync(specialtyId);
        if (list.Count == 0 || plan == 0) return 0m;

        var take = Math.Min(plan, list.Count);
        return list[take - 1].TotalScore;
    }

    /// <summary>
    /// Динамика подачи заявлений по дням и специальностям с расчётом конкурса.
    /// </summary>
    public async Task<List<ApplicationDynamicsDto>> GetApplicationDynamicsAsync(DateTime from, DateTime to)
    {
        var applications = await _db.Applications
            .AsNoTracking()
            .Include(a => a.Specialty)
            .Where(a => a.SubmissionDateTime >= from && a.SubmissionDateTime <= to)
            .ToListAsync();

        var result = applications
            .GroupBy(a => new { Date = a.SubmissionDateTime.Date, a.SpecialtyId, a.Specialty.Name })
            .Select(g => new ApplicationDynamicsDto
            {
                Date = g.Key.Date,
                SpecialtyId = g.Key.SpecialtyId,
                SpecialtyName = g.Key.Name,
                ApplicationsCount = g.Count(),
                BudgetPlan = 0,
                PaidPlan = 0,
                Competition = 0
            })
            .OrderBy(x => x.Date)
            .ThenBy(x => x.SpecialtyName)
            .ToList();

        // Дозаполняем планы и конкурс
        foreach (var row in result)
        {
            var spec = await _db.Specialties
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SpecialtyId == row.SpecialtyId);

            if (spec is null) continue;

            row.BudgetPlan = spec.BudgetPlan;
            row.PaidPlan = spec.PaidPlan;

            var totalPlaces = spec.BudgetPlan + spec.PaidPlan;
            row.Competition = totalPlaces == 0
                ? 0
                : Math.Round((decimal)row.ApplicationsCount / totalPlaces, 2);
        }

        return result;
    }

    /// <summary>
    /// Отчёт о выполнении плана приёма по факультетам, формам обучения и формам финансирования.
    /// </summary>
    public async Task<List<AdmissionPlanReportDto>> GetAdmissionPlanReportAsync()
    {
        var specialties = await _db.Specialties
            .AsNoTracking()
            .Include(s => s.Faculty)
            .ToListAsync();

        var enrollments = await _db.OrderEnrollments
            .AsNoTracking()
            .Include(e => e.Application)
                .ThenInclude(a => a.Specialty)
                    .ThenInclude(s => s.Faculty)
            .ToListAsync();

        var result = new List<AdmissionPlanReportDto>();

        foreach (var spec in specialties)
        {
            // Бюджет
            if (spec.BudgetPlan > 0)
            {
                var enrolledBudget = enrollments.Count(e =>
                    e.Application.SpecialtyId == spec.SpecialtyId &&
                    e.FundingForm == FundingForm.Бюджет);

                result.Add(new AdmissionPlanReportDto
                {
                    FacultyName = spec.Faculty.Name,
                    EducationForm = spec.EducationForm.ToString(),
                    FundingForm = FundingForm.Бюджет.ToString(),
                    Plan = spec.BudgetPlan,
                    Enrolled = enrolledBudget
                });
            }

            // Платно
            if (spec.PaidPlan > 0)
            {
                var enrolledPaid = enrollments.Count(e =>
                    e.Application.SpecialtyId == spec.SpecialtyId &&
                    e.FundingForm == FundingForm.Платно);

                result.Add(new AdmissionPlanReportDto
                {
                    FacultyName = spec.Faculty.Name,
                    EducationForm = spec.EducationForm.ToString(),
                    FundingForm = FundingForm.Платно.ToString(),
                    Plan = spec.PaidPlan,
                    Enrolled = enrolledPaid
                });
            }
        }

        return result
            .OrderBy(x => x.FacultyName)
            .ThenBy(x => x.EducationForm)
            .ThenBy(x => x.FundingForm)
            .ToList();
    }
}