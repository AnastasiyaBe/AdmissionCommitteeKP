namespace AdmissionCommittee.Application.DTO;

public class ApplicantSearchDto
{
    public ApplicantDto Applicant { get; set; } = null!;
    public List<ApplicationDto> Applications { get; set; } = new();
    public List<TestResultDto> TestResults { get; set; } = new();
}