using System;
using System.Collections.Generic;

namespace test.Models;

public partial class StudentLesson
{
    public string StudentId { get; set; } = null!;

    public string LessonId { get; set; } = null!;

    public DateTime? CompletedDatetime { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
