using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? IsProgressLimited { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public virtual ICollection<Module> Modules { get; } = new List<Module>();

    public virtual ICollection<Quiz> Quizzes { get; } = new List<Quiz>();
}
