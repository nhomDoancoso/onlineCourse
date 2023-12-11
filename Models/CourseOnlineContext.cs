using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

public partial class CourseOnlineContext : DbContext
{
    public CourseOnlineContext()
    {
    }

    public CourseOnlineContext(DbContextOptions<CourseOnlineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAnswer> QuizAnswers { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentLesson> StudentLessons { get; set; }

    public virtual DbSet<StudentQuizAttempt> StudentQuizAttempts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=CourseOnline;user=root;password=hiep2410", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId)
                .HasMaxLength(50)
                .HasColumnName("course_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IsProgressLimited).HasColumnName("is_progress_limited");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Price)
                .HasPrecision(10, 3)
                .HasColumnName("price");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.StudentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("enrollment");

            entity.HasIndex(e => e.StudentId, "student_id");

            entity.Property(e => e.CourseId)
                .HasMaxLength(50)
                .HasColumnName("course_id");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .HasColumnName("student_id");
            entity.Property(e => e.CompletedDatetime)
                .HasColumnType("datetime")
                .HasColumnName("completed_datetime");
            entity.Property(e => e.EnrollmentDatetime)
                .HasColumnType("datetime")
                .HasColumnName("enrollment_datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollment_ibfk_1");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollment_ibfk_2");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PRIMARY");

            entity.ToTable("lesson");

            entity.HasIndex(e => e.ModuleId, "module_id");

            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .HasColumnName("lesson_id");
            entity.Property(e => e.CourseOrder).HasColumnName("course_order");
            entity.Property(e => e.LessonDetails)
                .HasMaxLength(255)
                .HasColumnName("lesson_details")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModuleId)
                .HasMaxLength(50)
                .HasColumnName("module_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("lesson_ibfk_1");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PRIMARY");

            entity.ToTable("module");

            entity.HasIndex(e => e.CourseId, "course_id");

            entity.Property(e => e.ModuleId)
                .HasMaxLength(50)
                .HasColumnName("module_id");
            entity.Property(e => e.CourseId)
                .HasMaxLength(50)
                .HasColumnName("course_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("module_ibfk_1");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PRIMARY");

            entity.ToTable("quiz");

            entity.HasIndex(e => e.CourseId, "course_id");

            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .HasColumnName("quiz_id");
            entity.Property(e => e.CourseId)
                .HasMaxLength(50)
                .HasColumnName("course_id");
            entity.Property(e => e.CourseOrder).HasColumnName("course_order");
            entity.Property(e => e.IsPassRequired).HasColumnName("is_pass_required");
            entity.Property(e => e.MinPassScore).HasColumnName("min_pass_score");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.Course).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("quiz_ibfk_1");
        });

        modelBuilder.Entity<QuizAnswer>(entity =>
        {
            entity.HasKey(e => e.QuizAnswerId).HasName("PRIMARY");

            entity.ToTable("quiz_answer");

            entity.HasIndex(e => e.QuizQuestionId, "quiz_question_id");

            entity.Property(e => e.QuizAnswerId)
                .HasMaxLength(50)
                .HasColumnName("quiz_answer_id");
            entity.Property(e => e.QuizQuestionId)
                .HasMaxLength(50)
                .HasColumnName("quiz_question_id");

            entity.HasOne(d => d.QuizQuestion).WithMany(p => p.QuizAnswers)
                .HasForeignKey(d => d.QuizQuestionId)
                .HasConstraintName("quiz_answer_ibfk_1");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.QuizQuestionId).HasName("PRIMARY");

            entity.ToTable("quiz_question");

            entity.HasIndex(e => e.QuizId, "quiz_id");

            entity.Property(e => e.QuizQuestionId)
                .HasMaxLength(50)
                .HasColumnName("quiz_question_id");
            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .HasColumnName("quiz_id");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("quiz_question_ibfk_1");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("students");

            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .HasColumnName("student_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .HasColumnName("email_address");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<StudentLesson>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.LessonId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("student_lessons");

            entity.HasIndex(e => e.LessonId, "lesson_id");

            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .HasColumnName("student_id");
            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .HasColumnName("lesson_id");
            entity.Property(e => e.CompletedDatetime)
                .HasColumnType("datetime")
                .HasColumnName("completed_datetime");

            entity.HasOne(d => d.Lesson).WithMany(p => p.StudentLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_lessons_ibfk_2");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentLessons)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_lessons_ibfk_1");
        });

        modelBuilder.Entity<StudentQuizAttempt>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.QuizId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("student_quiz_attempt");

            entity.HasIndex(e => e.QuizId, "quiz_id");

            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .HasColumnName("student_id");
            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .HasColumnName("quiz_id");
            entity.Property(e => e.AttemptDatetime)
                .HasColumnType("datetime")
                .HasColumnName("attempt_datetime");
            entity.Property(e => e.ScoreAchieved).HasColumnName("score_achieved");

            entity.HasOne(d => d.Quiz).WithMany(p => p.StudentQuizAttempts)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_quiz_attempt_ibfk_2");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentQuizAttempts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_quiz_attempt_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
