using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface IApplicationService
{
    Task<List<ApplicationDto>> GetAllAsync();
    Task<ApplicationDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ApplicationDto dto);
    Task UpdateAsync(ApplicationDto dto);
    Task DeleteAsync(int id);
}