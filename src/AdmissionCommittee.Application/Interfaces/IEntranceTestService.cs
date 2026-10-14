using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface IEntranceTestService
{
    Task<List<EntranceTestDto>> GetAllAsync();
    Task<EntranceTestDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(EntranceTestDto dto);
    Task UpdateAsync(EntranceTestDto dto);
    Task DeleteAsync(int id);
}