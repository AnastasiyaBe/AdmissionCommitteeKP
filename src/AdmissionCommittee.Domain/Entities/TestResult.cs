using System;
using System.Collections.Generic;

namespace AdmissionCommittee.Domain.Entities;

public partial class TestResult
{
    public int ResultId { get; set; }

    public int ApplicantId { get; set; }

    public int TestId { get; set; }

    public decimal Score { get; set; }

    public DateTime ResultDate { get; set; }

    public string CertificateNumber { get; set; } = null!;

    public virtual Applicant Applicant { get; set; } = null!;

    public virtual EntranceTest Test { get; set; } = null!;
}
