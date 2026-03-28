using System.Security.Claims;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplate;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/workout-templates")]
    public class WorkoutTemplateController(IWorkoutTemplateService workoutTemplateService) : ControllerBase
    {
        private readonly IWorkoutTemplateService _workoutTemplateService = workoutTemplateService;

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutTemplates()
        {
            ICollection<WorkoutTemplateResponseDto> workoutTemplates = await _workoutTemplateService.GetWorkoutTemplatesAsync();
            return Ok(workoutTemplates);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutTemplate([FromBody] CreateWorkoutTemplateDto createWorkoutTemplateDto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            WorkoutTemplateResponseDto workoutTemplate = await _workoutTemplateService.CreateWorkoutTemplateAsync(createWorkoutTemplateDto, userId);
            return Ok(workoutTemplate);
        }

        [HttpPatch("{workoutTemplateId}")]
        public async Task<IActionResult> UpdateWorkoutTemplate([FromBody] UpdateWorkoutTemplateDto updateWorkoutTemplateDto, string workoutTemplateId)
        {
            WorkoutTemplateResponseDto workoutTemplate = await _workoutTemplateService.UpdateWorkoutTemplateAsync(updateWorkoutTemplateDto, Guid.Parse(workoutTemplateId));
            return Ok(workoutTemplate);
        }

        [HttpDelete("{workoutTemplateId}")]
        public async Task<IActionResult> DeleteWorkoutTemplate(string workoutTemplateId)
        {
            DeleteResponseMessageDto response = await _workoutTemplateService.DeleteWorkoutTemplateAsync(Guid.Parse(workoutTemplateId));
            return Ok(response);
        }

        [HttpGet("{workoutTemplateId}")]
        public async Task<IActionResult?> GetWorkoutTemplateByIdWithExercises(string workoutTemplateId)
        {
            WorkoutTemplateWithExercisesDto? workoutTemplateWithExercises = await _workoutTemplateService.GetWorkoutTemplateByIdWithExercisesAsync(Guid.Parse(workoutTemplateId));
            return Ok(workoutTemplateWithExercises);
        }

    }
}
