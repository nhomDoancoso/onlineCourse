using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Quiz
{
    public string QuizId { get; set; } = null!;

    public string? CourseId { get; set; }

    public string? Name { get; set; }

    public int? Number { get; set; }

    public int? CourseOrder { get; set; }

    public int? MinPassScore { get; set; }

    public bool? IsPassRequired { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<QuizQuestion> QuizQuestions { get; } = new List<QuizQuestion>();

    public virtual ICollection<StudentQuizAttempt> StudentQuizAttempts { get; } = new List<StudentQuizAttempt>();
}
