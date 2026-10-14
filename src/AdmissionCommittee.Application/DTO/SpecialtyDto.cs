namespace AdmissionCommittee.Application.DTO;

public class SpecialtyDto
{
    public int SpecialtyId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int FacultyId { get; set; }
    public string FacultyName { get; set; } = null!;
    public string EducationForm { get; set; } = null!;
    public decimal DurationYears { get; set; }
    public int BudgetPlan { get; set; }
    public int PaidPlan { get; set; }
}