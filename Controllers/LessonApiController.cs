using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonApiController : Controller
    {
        private readonly CourseOnlineContext _courseOnlineContext;

        public LessonApiController(CourseOnlineContext courseOnlineContext)
        {
            _courseOnlineContext = courseOnlineContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessons()
        {
            var lessons = await _courseOnlineContext.Lessons
                .Include(x => x.Module)
                .Include(x => x.StudentLessons)
                .ToListAsync();

            var lessonList = lessons.Select(lesson => new
            {
                lesson.LessonId,
                lesson.ModuleId,
                lesson.Name,
                lesson.Number,
                lesson.VideoUrl,
                lesson.LessonDetails,
                lesson.CourseOrder,
                Module = new
                {
                    ModuleId = lesson.Module?.ModuleId,
                    CourseId = lesson.Module?.CourseId,
                    ModuleName = lesson.Module?.Name,
                    ModuleNumber = lesson.Module?.Number,
                },
            }).ToList();

            return Ok(lessonList);
        }
    }
}
