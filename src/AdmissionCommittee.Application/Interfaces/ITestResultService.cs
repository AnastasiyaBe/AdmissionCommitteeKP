using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface ITestResultService
{
    Task<List<TestResultDto>> GetAllAsync();
    Task<TestResultDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(TestResultDto dto);
    Task UpdateAsync(TestResultDto dto);
    Task DeleteAsync(int id);
}