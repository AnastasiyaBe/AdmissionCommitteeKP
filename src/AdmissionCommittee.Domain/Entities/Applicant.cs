using System;
using System.Collections.Generic;

namespace AdmissionCommittee.Domain.Entities;

public partial class Applicant
{
    public int ApplicantId { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Passport { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string EducationDocument { get; set; } = null!;

    public decimal AverageScore { get; set; }

    public string? Benefits { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
