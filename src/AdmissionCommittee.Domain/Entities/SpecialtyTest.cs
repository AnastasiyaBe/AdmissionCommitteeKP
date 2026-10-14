using System;
using System.Collections.Generic;

namespace AdmissionCommittee.Domain.Entities;

public partial class SpecialtyTest
{
    public int SpecialtyId { get; set; }

    public int TestId { get; set; }

    public decimal Weight { get; set; }

    public virtual Specialty Specialty { get; set; } = null!;

    public virtual EntranceTest Test { get; set; } = null!;
}
