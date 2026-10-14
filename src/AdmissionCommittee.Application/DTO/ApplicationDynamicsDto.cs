namespace AdmissionCommittee.Application.DTO;

public class ApplicationDynamicsDto
{
    public DateTime Date { get; set; }
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = null!;
    public int ApplicationsCount { get; set; }
    public int BudgetPlan { get; set; }
    public int PaidPlan { get; set; }
    public decimal Competition { get; set; }
}