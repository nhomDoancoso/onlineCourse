using System;
using System.Collections.Generic;

namespace test.Models;

public partial class StudentQuizAttempt
{
    public string StudentId { get; set; } = null!;

    public string QuizId { get; set; } = null!;

    public DateTime? AttemptDatetime { get; set; }

    public int? ScoreAchieved { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
