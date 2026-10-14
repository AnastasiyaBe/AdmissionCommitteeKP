using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface IReportService
{
    Task<List<CompetitiveListItemDto>> GetCompetitiveListAsync(int specialtyId, string? fundingForm = null);
    Task<decimal> GetCurrentPassingScoreAsync(int specialtyId);
    Task<List<ApplicationDynamicsDto>> GetApplicationDynamicsAsync(DateTime from, DateTime to);
    Task<List<AdmissionPlanReportDto>> GetAdmissionPlanReportAsync();
}