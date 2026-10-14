using AdmissionCommittee.Application.DTO;
using AdmissionCommittee.Application.Interfaces;
using AdmissionCommittee.Domain.Entities;
using AdmissionCommittee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application = AdmissionCommittee.Domain.Entities.Application;

namespace AdmissionCommittee.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly AdmissionCommitteeDbContext _db;

    public OrderService(AdmissionCommitteeDbContext db)
    {
        _db = db;
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        return await _db.Orders
            .AsNoTracking()
            .Include(o => o.OrderEnrollments)
                .ThenInclude(e => e.Application)
                    .ThenInclude(a => a.Applicant)
            .Include(o => o.OrderEnrollments)
                .ThenInclude(e => e.Application)
                    .ThenInclude(a => a.Specialty)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Enrollments = o.OrderEnrollments.Select(e => new OrderEnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    ApplicationId = e.ApplicationId,
                    ApplicantFullName = e.Application.Applicant.FullName,
                    SpecialtyName = e.Application.Specialty.Name,
                    FundingForm = e.FundingForm.ToString()
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var o = await _db.Orders
            .AsNoTracking()
            .Include(x => x.OrderEnrollments)
                .ThenInclude(e => e.Application)
                    .ThenInclude(a => a.Applicant)
            .Include(x => x.OrderEnrollments)
                .ThenInclude(e => e.Application)
                    .ThenInclude(a => a.Specialty)
            .FirstOrDefaultAsync(x => x.OrderId == id);

        if (o is null) return null;

        return new OrderDto
        {
            OrderId = o.OrderId,
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            Enrollments = o.OrderEnrollments.Select(e => new OrderEnrollmentDto
            {
                EnrollmentId = e.EnrollmentId,
                ApplicationId = e.ApplicationId,
                ApplicantFullName = e.Application.Applicant.FullName,
                SpecialtyName = e.Application.Specialty.Name,
                FundingForm = e.FundingForm.ToString()
            }).ToList()
        };
    }

    public async Task<int> CreateAsync(OrderDto dto)
    {
        var entity = new Order
        {
            OrderNumber = dto.OrderNumber,
            OrderDate = dto.OrderDate
        };

        _db.Orders.Add(entity);
        await _db.SaveChangesAsync();
        return entity.OrderId;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Orders.FindAsync(id);
        if (entity is null) return;

        _db.Orders.Remove(entity);
        await _db.SaveChangesAsync();
    }
}