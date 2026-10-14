using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface ISpecialtyService
{
    Task<List<SpecialtyDto>> GetAllAsync();
    Task<SpecialtyDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(SpecialtyDto dto);
    Task UpdateAsync(SpecialtyDto dto);
    Task DeleteAsync(int id);
}