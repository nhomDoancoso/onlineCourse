using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers;

[Controller]
[Route("api/[controller]")]
public class StudentApiController : Controller
{
    private readonly CourseOnlineContext _courseOnlineContext;
    public StudentApiController(CourseOnlineContext courseOnlineContext)
    {
        _courseOnlineContext = courseOnlineContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudent() 
    {
        var students = await _courseOnlineContext.Students
            .Include(s => s.Enrollments)
            .Include(s => s.StudentLessons)
            .Include(s => s.StudentQuizAttempts)
            .ToListAsync();

        var studentsWithFullInfo = students.Select(s => new
        {
            s.StudentId,
            s.EmailAddress,
            s.PasswordHash,
            s.FirstName,
            s.LastName,
            s.Address,
            s.Phone,
            s.AvatarUrl
        }).ToList();

        return Ok(studentsWithFullInfo);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateStudent(Student student)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var newStudent = new Student
                {
                    EmailAddress = student.EmailAddress,
                    PasswordHash = student.PasswordHash,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Address = student.Address,
                    Phone = student.Phone,
                    AvatarUrl = student.AvatarUrl,
                };

                _courseOnlineContext.Students.Add(newStudent);
                await _courseOnlineContext.SaveChangesAsync();

                var registrationSuccessResponse = new
                {
                    Message = "Student created successfully",
                    StudentId = newStudent.StudentId,
                    EmailAddress = newStudent.EmailAddress,
                    FirstName = newStudent.FirstName,
                    LastName = newStudent.LastName,
                    Address = newStudent.Address,
                    Phone = newStudent.Phone,
                    AvatarUrl = newStudent.AvatarUrl,
                };

                return Ok(registrationSuccessResponse);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, new { Message = "An error occurred while processing the request", Error = ex.Message });
            }
        }

        var invalidDataErrorResponse = new
        {
            Message = "Invalid create data",
            Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList()
        };

        return BadRequest(invalidDataErrorResponse);
    }


}