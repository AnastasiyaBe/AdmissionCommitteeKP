namespace AdmissionCommittee.Application.DTO;

public class ApplicationDto
{
    public int ApplicationId { get; set; }
    public string ApplicationNumber { get; set; } = null!;
    public int ApplicantId { get; set; }
    public string ApplicantFullName { get; set; } = null!;
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = null!;
    public int Priority { get; set; }
    public DateTime SubmissionDateTime { get; set; }
    public string Status { get; set; } = null!;
}