using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseApiController : Controller
{
    private readonly CourseOnlineContext _courseOnlineContext;

    public CourseApiController(CourseOnlineContext courseOnlineContext) {
        _courseOnlineContext = courseOnlineContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourse() {
        var course = await _courseOnlineContext.Courses
            .Include(s => s.Enrollments)
            .Include(s => s.Modules)
            .Include(s => s.Quizzes)
            .ToListAsync();

        var coursesWithFullInfo = course.Select(s => new
        {
            s.CourseId,
            s.Name,
            s.Description,
            s.Price,
            s.IsProgressLimited,
        }).ToList();

        return Ok(coursesWithFullInfo);
    }
    
    [HttpGet("GetCourseId/{courseId}")]
    public async Task<IActionResult> GetCourseById(string courseId) {
        if(string.IsNullOrEmpty(courseId)) {
            return BadRequest("Invalid courseId");
        }
        
        var course = await _courseOnlineContext.Courses
            .Include(s => s.Enrollments)
            .Include(s => s.Modules)
            .Include(s => s.Quizzes)
            .FirstOrDefaultAsync(x => x.CourseId == courseId);

        if(course == null) {
            return NotFound();
        }

        var coursesWithFullInfo = new {
            course.CourseId,
            course.Name,
            course.Description,
            course.Price,
            course.IsProgressLimited
        };
        return Ok(coursesWithFullInfo);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCourse(Course course) {
        if(ModelState.IsValid) {
            try {
                _courseOnlineContext.Courses.Add(course);
                await _courseOnlineContext.SaveChangesAsync();
            } catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing the request",
                    Error = e.Message,
                    InnerError = e.InnerException?.Message
                });
            }

        }
        var CreateCourseSuccess = new
            {
                Message = "create successful",
                CourseId = course.CourseId,
                Name = course.Name,
                Description = course.Description,
                IsProgressLimited = course.IsProgressLimited
            };
        return Ok(CreateCourseSuccess);
    }

    [HttpPut("update/{courseId}")]
    public async Task<IActionResult> UpdateCourse(string courseId, Course course) {
        var courseUpdate = await _courseOnlineContext.Courses
            .FirstOrDefaultAsync(x => x.CourseId == courseId);
        
        if(course == null) {
            return NotFound();
        }

        courseUpdate.Name = course.Name;
        courseUpdate.Description = course.Description;
        courseUpdate.Price = course.Price;
        course.IsProgressLimited = course.IsProgressLimited;

        _courseOnlineContext.Entry(courseUpdate).State = EntityState.Modified;
        await _courseOnlineContext.SaveChangesAsync();

        var updateSuccessResponse = new {
            message = "Course updated success",
            name = course.Name,
            description = course.Description,
            price = course.Price,
            isProgressLimited = course.IsProgressLimited  
        };

        return Ok(updateSuccessResponse);
    }

    [HttpDelete("delete/{courseId}")]
    public async Task<IActionResult> DeleteCourse(string courseId) {
        var courseDelete = await _courseOnlineContext.Courses.FindAsync(courseId);

        if(courseDelete == null) {
            return NotFound();
        }

        _courseOnlineContext.Courses.Remove(courseDelete);
        await _courseOnlineContext.SaveChangesAsync();

        var deleteSuccessResponse = new {
            Message = "Course Delete successfully"
        };

        return Ok(deleteSuccessResponse);
    }
}