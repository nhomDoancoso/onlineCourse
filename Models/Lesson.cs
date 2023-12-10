using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Lesson
{
    public string LessonId { get; set; } = null!;

    public string? ModuleId { get; set; }

    public string? Name { get; set; }

    public int? Number { get; set; }

    public string? VideoUrl { get; set; }

    public string? LessonDetails { get; set; }

    public int? CourseOrder { get; set; }

    public virtual Module? Module { get; set; }

    public virtual ICollection<StudentLesson> StudentLessons { get; } = new List<StudentLesson>();
}
