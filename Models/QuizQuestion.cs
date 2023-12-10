using System;
using System.Collections.Generic;

namespace test.Models;

public partial class QuizQuestion
{
    public string QuizQuestionId { get; set; } = null!;

    public string? QuizId { get; set; }

    public virtual Quiz? Quiz { get; set; }

    public virtual ICollection<QuizAnswer> QuizAnswers { get; } = new List<QuizAnswer>();
}
