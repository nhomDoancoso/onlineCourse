using System;
using System.Collections.Generic;

namespace test.Models;

public partial class QuizAnswer
{
    public string QuizAnswerId { get; set; } = null!;

    public string? QuizQuestionId { get; set; }

    public virtual QuizQuestion? QuizQuestion { get; set; }
}
