using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers;
[Controller]
[Route("api/[controller]")]
public class CourseApiController : Controller
{
    
    private readonly CourseOnlineContext _courseOnlineContext;
    public CourseApiController(CourseOnlineContext courseOnlineContext)
    {
        _courseOnlineContext = courseOnlineContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCombo()
    {
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
}