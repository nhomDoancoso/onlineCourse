using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Enrollment
{
    public string CourseId { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public DateTime? EnrollmentDatetime { get; set; }

    public DateTime? CompletedDatetime { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
