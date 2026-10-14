namespace AdmissionCommittee.Application.DTO;

public class OrderDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public List<OrderEnrollmentDto> Enrollments { get; set; } = new();
}

public class OrderEnrollmentDto
{
    public int EnrollmentId { get; set; }
    public int ApplicationId { get; set; }
    public string ApplicantFullName { get; set; } = null!;
    public string SpecialtyName { get; set; } = null!;
    public string FundingForm { get; set; } = null!;
}