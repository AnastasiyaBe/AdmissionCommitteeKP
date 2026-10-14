using System;
using System.Collections.Generic;

namespace AdmissionCommittee.Domain.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public virtual ICollection<OrderEnrollment> OrderEnrollments { get; set; } = new List<OrderEnrollment>();
}
