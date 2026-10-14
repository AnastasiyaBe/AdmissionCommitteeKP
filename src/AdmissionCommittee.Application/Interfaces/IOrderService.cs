using AdmissionCommittee.Application.DTO;

namespace AdmissionCommittee.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(OrderDto dto);
    Task DeleteAsync(int id);
}