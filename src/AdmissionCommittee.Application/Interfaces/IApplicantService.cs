using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface IApplicantService
{
    Task<List<ApplicantDto>> GetAllAsync();
    Task<ApplicantDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ApplicantDto dto);
    Task UpdateAsync(ApplicantDto dto);
    Task DeleteAsync(int id);

    Task<ApplicantSearchDto?> SearchAsync(string query);
}