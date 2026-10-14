using System;
using System.Collections.Generic;

namespace AdmissionCommittee.Domain.Entities;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
}
