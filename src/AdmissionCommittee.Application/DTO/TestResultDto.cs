namespace AdmissionCommittee.Application.DTO;

public class TestResultDto
{
    public int ResultId { get; set; }
    public int ApplicantId { get; set; }
    public string ApplicantFullName { get; set; } = null!;
    public int TestId { get; set; }
    public string TestName { get; set; } = null!;
    public decimal Score { get; set; }
    public DateTime ResultDate { get; set; }
    public string CertificateNumber { get; set; } = null!;
}