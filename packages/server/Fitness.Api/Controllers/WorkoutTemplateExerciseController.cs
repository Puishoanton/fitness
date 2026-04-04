using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplateExercise;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/workout-template-exercises")]
    public class WorkoutTemplateExerciseController(IWorkoutTemplateExerciseService workoutTemplateExerciseService) : ControllerBase
    {
        private readonly IWorkoutTemplateExerciseService _workoutTemplateExerciseService = workoutTemplateExerciseService;

        [HttpGet]
        public async Task<IActionResult> GetWorkoutTemplateExercisesByWorkoutTemplateId(Guid workoutTemplateId)
        {
            List<WorkoutTemplateExerciseResponseDto> wte = await _workoutTemplateExerciseService.GetWorkoutTemplateExercisesByWorkoutTemplateIdAsync(workoutTemplateId);
            return Ok(wte);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutTemplateExercisesByWorkoutTemplateId(Guid workoutTemplateId, [FromBody] CreateWorkoutTemplateExerciseDto createWorkoutTemplateExerciseDto)
        {
            WorkoutTemplateExerciseResponseDto wte = await _workoutTemplateExerciseService.CreateWorkoutTemplateExerciseAsync(workoutTemplateId, createWorkoutTemplateExerciseDto);
            return Ok(wte);
        }

        [HttpPatch("{workoutTemplateExerciseId}")]
        public async Task<IActionResult> UpdateWorkoutTemplateExercise(Guid workoutTemplateExerciseId, [FromBody] UpdateWorkoutTemplateExerciseDto updateWorkoutTemplateExerciseDto)
        {
            WorkoutTemplateExerciseResponseDto wte = await _workoutTemplateExerciseService.UpdateWorkoutTemplateExerciseAsync(workoutTemplateExerciseId, updateWorkoutTemplateExerciseDto);
            return Ok(wte);
        }

        [HttpDelete("{workoutTemplateExerciseId}")]
        public async Task<IActionResult> DeleteWorkoutTemplateExercise(Guid workoutTemplateExerciseId)
        {
            DeleteResponseMessageDto response = await _workoutTemplateExerciseService.DeleteWorkoutTemplateExerciseAsync(workoutTemplateExerciseId);
            return Ok(response);
        }

        [HttpPatch("move/{workoutTemplateExerciseId}")]
        public async Task<IActionResult> MoveWorkoutTemplateExercise(Guid workoutTemplateExerciseId, [FromBody] int newOrder)
        {
            await _workoutTemplateExerciseService.MoveWorkoutTemplateExerciseAsync(workoutTemplateExerciseId, newOrder);
            return Ok();
        }
    }
}
