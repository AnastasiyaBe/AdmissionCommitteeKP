namespace AdmissionCommittee.Application.DTO;

public class AdmissionPlanReportDto
{
    public string FacultyName { get; set; } = null!;
    public string EducationForm { get; set; } = null!;
    public string FundingForm { get; set; } = null!;
    public int Plan { get; set; }
    public int Enrolled { get; set; }
    public int Remaining => Plan - Enrolled;
    public decimal CompletionPercent => Plan == 0 ? 0 : Math.Round((decimal)Enrolled / Plan * 100, 2);
}