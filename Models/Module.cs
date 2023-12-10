using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Module
{
    public string ModuleId { get; set; } = null!;

    public string? CourseId { get; set; }

    public string? Name { get; set; }

    public int? Number { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Lesson> Lessons { get; } = new List<Lesson>();
}
