namespace AdmissionCommittee.Application.DTO;

public class CompetitiveListItemDto
{
    public int ApplicationId { get; set; }
    public string ApplicationNumber { get; set; } = null!;
    public int ApplicantId { get; set; }
    public string ApplicantFullName { get; set; } = null!;
    public decimal AverageScore { get; set; }
    public decimal TestsScore { get; set; }
    public decimal TotalScore { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; } = null!;
}