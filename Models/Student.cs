using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string? PasswordHash { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();

    public virtual ICollection<StudentLesson> StudentLessons { get; } = new List<StudentLesson>();

    public virtual ICollection<StudentQuizAttempt> StudentQuizAttempts { get; } = new List<StudentQuizAttempt>();
    
}
