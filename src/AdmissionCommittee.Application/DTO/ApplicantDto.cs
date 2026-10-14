namespace AdmissionCommittee.Application.DTO;

public class ApplicantDto
{
    public int ApplicantId { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Passport { get; set; } = null!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string EducationDocument { get; set; } = null!;
    public decimal AverageScore { get; set; }
    public string? Benefits { get; set; }
}