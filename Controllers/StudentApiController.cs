using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentApiController : Controller
{
    private readonly CourseOnlineContext _courseOnlineContext;

    public StudentApiController(CourseOnlineContext courseOnlineContext) {
        _courseOnlineContext = courseOnlineContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudent() {
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
    public async Task<IActionResult> CreateStudent(Student student) {
        if (ModelState.IsValid)
        {
            try
            {
               _courseOnlineContext.Students.Add(student);
                await _courseOnlineContext.SaveChangesAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
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