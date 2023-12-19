using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleApiController : Controller
{
    private readonly CourseOnlineContext _courseOnlineContext;

    public ModuleApiController(CourseOnlineContext courseOnlineContext) {
        _courseOnlineContext = courseOnlineContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllModule() {
        var module = await _courseOnlineContext.Modules
            .Include(x => x.Course)
            .ToListAsync();

        var moduleFullInfo = module.Select(s => new {
            s.ModuleId,
            s.CourseId,
            s.Number,
            Course = new {
                courseId = s.Course?.CourseId,
                Name = s.Course?.Name,
                Description = s.Course?.Description,
                Price = s.Course?.Price,
                 isProgrees = s.Course?.IsProgressLimited,
            }
        });
        return Ok(moduleFullInfo);
    }

    [HttpGet("getModuleById/{moduleId}")]
    public async Task<IActionResult> GetModuleById(string moduleId) {
        var moduleById = await _courseOnlineContext.Modules
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.ModuleId == moduleId);

        if (moduleById == null)
        {
            return NotFound();
        }

        var moduleFullInfo = new {
            moduleById.ModuleId,
            moduleById.CourseId,
            moduleById.Number,
            Course = new {
                courseId = moduleById.Course?.CourseId,
                Name = moduleById.Course?.Name,
                Description = moduleById.Course?.Description,
                Price = moduleById.Course?.Price,
                isProgrees = moduleById.Course?.IsProgressLimited,
            }
        };
        return Ok(moduleFullInfo);
    }

    [HttpPost("create")] 
    public async Task<IActionResult> createModule(Module module) {
        if (ModelState.IsValid)
            {
                var course = await _courseOnlineContext.Courses.FindAsync(module.CourseId);

                var newModule= new Module
                {
                    ModuleId = module.ModuleId,
                    CourseId = module.CourseId,
                    Name = module.Name,
                    Number = module.Number,
                    Course = course,
                };

                _courseOnlineContext.Modules.Add(newModule);
                await _courseOnlineContext.SaveChangesAsync();
                _courseOnlineContext.Entry(newModule).Reference(s => s.Course).Load();

                var registrationSuccessResponse = new
                {
                    Message = "Module created successfully",
                    ModuleId = newModule.ModuleId,
                    Name = newModule.Name,
                    Number = newModule.Number,
                    Course = new
                    {
                        Name = newModule.Course?.Name,
                        Description = newModule.Course?.Description,
                        Price = newModule.Course?.Price,
                        IsProgressLimited = newModule.Course?.IsProgressLimited
                    },
                   
                };

                return Ok(registrationSuccessResponse);
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

    [HttpPut("update/{moduleId}")]
    public async Task<IActionResult> UpdateModule(string moduleId, Module updateModel)
    {
        try
        {
            var moduleToUpdate = await _courseOnlineContext.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

            if (moduleToUpdate == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateModel.CourseId))
            {
                var updatedCourse = await _courseOnlineContext.Courses.FindAsync(updateModel.CourseId);
                if (updatedCourse != null)
                {
                    moduleToUpdate.Course = updatedCourse;
                }
            }

            moduleToUpdate.Name = updateModel.Name;
            moduleToUpdate.Number = updateModel.Number;

            _courseOnlineContext.Entry(moduleToUpdate).State = EntityState.Modified;
            await _courseOnlineContext.SaveChangesAsync();

            var updateSuccessResponse = new
            {
                Message = "Module updated successfully"
            };

            return Ok(updateSuccessResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while processing the request", Error = ex.Message });
        }
    }


    [HttpDelete("delete/{moduleId}")]
    public async Task<IActionResult> DeleteModule(string moduleId)
    {
        var module = await _courseOnlineContext.Modules.FindAsync(moduleId);

        if (module == null)
        {
            return NotFound(new
            {
                Message = "Module not found",
            });
        }

        _courseOnlineContext.Modules.Remove(module);
        await _courseOnlineContext.SaveChangesAsync();

        var deleteSuccessResponse = new
        {
            Message = "module deleted successfully"
        };

        return Ok(deleteSuccessResponse);
    }
}