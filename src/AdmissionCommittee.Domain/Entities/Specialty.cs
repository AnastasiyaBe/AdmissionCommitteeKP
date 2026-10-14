using AdmissionCommittee.Domain.Enums;

namespace AdmissionCommittee.Domain.Entities;

public partial class Specialty
{
    public int SpecialtyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int FacultyId { get; set; }

    public EducationForm EducationForm { get; set; }

    public decimal DurationYears { get; set; }

    public int BudgetPlan { get; set; }

    public int PaidPlan { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<SpecialtyTest> SpecialtyTests { get; set; } = new List<SpecialtyTest>();
}