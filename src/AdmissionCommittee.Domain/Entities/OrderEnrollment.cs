using AdmissionCommittee.Domain.Enums;

namespace AdmissionCommittee.Domain.Entities;

public partial class OrderEnrollment
{
    public int EnrollmentId { get; set; }

    public int OrderId { get; set; }

    public int ApplicationId { get; set; }

    public FundingForm FundingForm { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}