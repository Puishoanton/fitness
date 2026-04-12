using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.Exercise;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/exercises")]
    public class ExerciseController(IExerciseService exerciseService) : ControllerBase
    {
        private readonly IExerciseService _exerciseService = exerciseService;

        [HttpGet]
        public async Task<IActionResult> GetAllExercises([FromQuery] MuscleGroup? muscleGroup)
        {
            ICollection<ExerciseLightDto> exercises = await _exerciseService.GetAllExercisesAsync(muscleGroup);
            return Ok(exercises);
        }
        [HttpGet("muscle-group")]
        public async Task<IActionResult> GetMuscleGroups()
        {
            List<MuscleGroup> muscleGroups = await _exerciseService.GetUniqueMuscleGroupsAsync();
            return Ok(muscleGroups);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseDto createExerciseDto)
        {
            ExerciseResponseDto exercise = await _exerciseService.CreateExerciseAsync(createExerciseDto);
            return Ok(exercise);
        }

        [HttpPatch("{exerciseId}")]
        public async Task<IActionResult> UpdateExercise([FromBody] UpdateExerciseDto updateExerciseDto, string exerciseId)
        {
            ExerciseResponseDto exercise = await _exerciseService.UpdateExerciseAsync(updateExerciseDto, Guid.Parse(exerciseId));
            return Ok(exercise);
        }

        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> DeleteExercise(string exerciseId)
        {
            DeleteResponseMessageDto response = await _exerciseService.DeleteExerciseAsync(Guid.Parse(exerciseId));
            return Ok(response);
        }

        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(string exerciseId)
        {
            ExerciseResponseDto? exercise = await _exerciseService.GetExerciseByIdAsync(Guid.Parse(exerciseId));
            return Ok(exercise);
        }
    }
}
