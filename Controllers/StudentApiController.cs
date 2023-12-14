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

    [HttpGet("getStudentById/{studentId}")]
    public async Task<IActionResult> GetStudentById(string studentId) 
    {
        if (string.IsNullOrEmpty(studentId))
        {
            return BadRequest("Invalid studentId");
        }

        var student = await _courseOnlineContext.Students
            .Include(s => s.Enrollments)
            .Include(s => s.StudentLessons)
            .Include(s => s.StudentQuizAttempts)
            .FirstOrDefaultAsync(s => s.StudentId == studentId);

        if (student == null)
        {
            return NotFound();
        }

        var studentWithFullInfo = new
        {
            student.StudentId,
            student.EmailAddress,
            student.PasswordHash,
            student.FirstName,
            student.LastName,
            student.Address,
            student.Phone,
            student.AvatarUrl
        };

        return Ok(studentWithFullInfo);
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

    [HttpPut("update/{studentId}")]
    public async Task<IActionResult> UpdateStudent(string studentId, Student student)
    {
        var studentUpdate = await _courseOnlineContext.Students
            .FirstOrDefaultAsync(p => p.StudentId == studentId);

        if (studentId == null)
        {
            return NotFound();
        }

        studentUpdate.EmailAddress = student.EmailAddress;
        studentUpdate.FirstName = student.FirstName;
        studentUpdate.LastName = student.LastName;
        studentUpdate.Address = student.Address;
        studentUpdate.Phone = student.Phone;
        studentUpdate.AvatarUrl = student.AvatarUrl;

        _courseOnlineContext.Entry(studentUpdate).State = EntityState.Modified;
        await _courseOnlineContext.SaveChangesAsync();

        var updateSuccessResponse = new
        {
            Message = "student updated successfully",
            emailAddress = student.EmailAddress,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Address = student.Address,
            Phone = student.Phone,
            AvatarUrl = student.AvatarUrl
        };

        return Ok(updateSuccessResponse);
    }

    [HttpDelete("delete/{studentId}")]
    public async Task<IActionResult> DeleteStudent(string studentId)
    {
        var studentDelete = await _courseOnlineContext.Students.FindAsync(studentId);

        if (studentDelete == null)
        {
            return NotFound();
        }

        _courseOnlineContext.Students.Remove(studentDelete);
        await _courseOnlineContext.SaveChangesAsync();

        var deleteSuccessResponse = new
        {
            Message = "Student deleted successfully"
        };

        return Ok(deleteSuccessResponse);
    }
}