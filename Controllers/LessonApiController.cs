using System.Net;
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
                    .ThenInclude(x => x.Course)
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
                Course = new 
                {
                    CourseId = lesson.Module?.Course?.CourseId,
                    Name = lesson.Module?.Course?.Name,
                    Description = lesson.Module?.Course?.Description,
                    Price = lesson.Module?.Course?.Price,
                    IsProgressLimited = lesson.Module?.Course?.IsProgressLimited,
                },
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
       
        [HttpGet("GetLessonById/{LessonId}")]
        public async Task<IActionResult> GetLessonById(string LessonId) {
            var lessons = await _courseOnlineContext.Lessons
            .Include(x => x.Module)
                .ThenInclude(x => x.Course)
            .FirstOrDefaultAsync(x => x.LessonId == LessonId); 

            if(lessons == null) {
                return NotFound();
            }

            var LessonByIdInfo = new {
                lessons.LessonId,
                lessons.ModuleId,
                lessons.Name,
                lessons.Number,
                lessons.VideoUrl,
                lessons.LessonDetails,
                lessons.CourseOrder,
                Course = new 
                {
                    CourseId = lessons.Module?.Course?.CourseId,
                    Name = lessons.Module?.Course?.Name,
                    Description = lessons.Module?.Course?.Description,
                    Price = lessons.Module?.Course?.Price,
                    IsProgressLimited = lessons.Module?.Course?.IsProgressLimited,
                },
                Module = new
                {
                    ModuleId = lessons.Module?.ModuleId,
                    CourseId = lessons.Module?.CourseId,
                    ModuleName = lessons.Module?.Name,
                    ModuleNumber = lessons.Module?.Number,
                },
            };
            return Ok(LessonByIdInfo); 
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLesson(Lesson createLesson)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var module = await _courseOnlineContext.Modules.FindAsync(createLesson.ModuleId);

                    if (module == null)
                    {
                        return NotFound(new { Message = "Module not found" });
                    }

                    var newLesson = new Lesson
                    {
                        LessonId = createLesson.LessonId,
                        ModuleId = createLesson.ModuleId,
                        Name = createLesson.Name,
                        Number = createLesson.Number,
                        VideoUrl = createLesson.VideoUrl,
                        LessonDetails = createLesson.LessonDetails,
                        CourseOrder = createLesson.CourseOrder,
                        Module = module,
                    };

                    _courseOnlineContext.Lessons.Add(newLesson);
                    await _courseOnlineContext.SaveChangesAsync();

                    _courseOnlineContext.Entry(newLesson).Reference(s => s.Module).Load();

                    var registrationSuccessResponse = new
                    {
                        Message = "Lesson created successfully",
                        LessonId = newLesson.LessonId,
                        ModuleId = newLesson.ModuleId,
                        Module = new
                        {
                            ModuleId = newLesson.Module?.ModuleId,
                            CourseId = newLesson.Module?.CourseId,
                            ModuleName = newLesson.Module?.Name,
                            ModuleNumber = newLesson.Module?.Number,
                        }
                    };

                    return Ok(registrationSuccessResponse);
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Error creating lesson", Error = ex.Message });
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

        [HttpPut("update/{lessonId}")]
        public async Task<IActionResult> UpdateLesson(string lessonId, Lesson updateLesson)
        {
            try
            {
                var lessonToUpdate = await _courseOnlineContext.Lessons
                    .Include(x => x.Module)
                    .ThenInclude(x => x.Course)
                    .FirstOrDefaultAsync(x => x.LessonId == lessonId);

                if (lessonToUpdate == null)
                {
                    return NotFound(new { Message = "Lesson not found" });
                }

                lessonToUpdate.Name = updateLesson.Name;
                lessonToUpdate.Number = updateLesson.Number;
                lessonToUpdate.VideoUrl = updateLesson.VideoUrl;
                lessonToUpdate.LessonDetails = updateLesson.LessonDetails;
                lessonToUpdate.CourseOrder = updateLesson.CourseOrder;

                if (lessonToUpdate.ModuleId != updateLesson.ModuleId) {
                    var newModule = await _courseOnlineContext.Modules.FindAsync(updateLesson.ModuleId);
                    if (newModule == null)
                    {
                        return NotFound(new { Message = "New Module not found" });
                    }
                    lessonToUpdate.ModuleId = updateLesson.ModuleId;
                    lessonToUpdate.Module = newModule;
                }

                _courseOnlineContext.Entry(lessonToUpdate).State = EntityState.Modified;
                await _courseOnlineContext.SaveChangesAsync();

                _courseOnlineContext.Entry(lessonToUpdate).Reference(x => x.Module).Load();

                var updateLessonResponse = new
                {
                    Message = "Lesson successfully updated",
                    LessonId = lessonToUpdate.LessonId,
                    ModuleId = lessonToUpdate.ModuleId,
                    Module = new
                    {
                        ModuleId = lessonToUpdate.Module?.ModuleId,
                        CourseId = lessonToUpdate.Module?.CourseId,
                        ModuleName = lessonToUpdate.Module?.Name,
                        ModuleNumber = lessonToUpdate.Module?.Number,
                    }
                };

                return Ok(updateLessonResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request", Error = ex.Message });
            }
        }

        [HttpDelete("delete/{lessonId}")]
        public async Task<IActionResult> DeleteLesson(string lessonId) {
            var lessons = await _courseOnlineContext.Lessons.FindAsync(lessonId);
            
            if(lessons == null) {
                return NotFound(new {
                   Message = "Lesson not Found" 
                });
            }

            _courseOnlineContext.Lessons.Remove(lessons);
            await _courseOnlineContext.SaveChangesAsync();

            var deleteSuccessResponse = new {
                Message = "lessons deleted successfully"
            };

            return Ok(deleteSuccessResponse);
        }
    }
}
