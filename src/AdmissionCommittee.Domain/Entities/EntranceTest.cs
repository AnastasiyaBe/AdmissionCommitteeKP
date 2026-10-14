using AdmissionCommittee.Domain.Enums;

namespace AdmissionCommittee.Domain.Entities;

public partial class EntranceTest
{
    public int TestId { get; set; }

    public string Name { get; set; } = null!;

    public TestForm Form { get; set; }

    public decimal MinScore { get; set; }

    public virtual ICollection<SpecialtyTest> SpecialtyTests { get; set; } = new List<SpecialtyTest>();

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}