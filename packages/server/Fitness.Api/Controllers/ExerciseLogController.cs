using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/exercise-logs")]
    public class ExerciseLogController(IExerciseLogService exerciseLogService) : ControllerBase
    {
        private readonly IExerciseLogService _exerciseLogService = exerciseLogService;

        [HttpGet]
        public async Task<IActionResult> GetAllExerciseLogs()
        {
            List<ExerciseLogLightDto> exerciseLogs = await _exerciseLogService.GetAllExerciseLogsAsync();
            return Ok(exerciseLogs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExerciseLog(string exerciseId, string workoutSessionId)
        {
            ExerciseLogLightDto exerciseLog = await _exerciseLogService.CreateExerciseLogAsync(Guid.Parse(exerciseId), Guid.Parse(workoutSessionId));
            return Ok(exerciseLog);
        }

        [HttpDelete("{exerciseLogId}")]
        public async Task<IActionResult> DeleteExerciseLog(string exerciseLogId)
        {
            DeleteResponseMessageDto response = await _exerciseLogService.DeleteExerciseLogAsync(Guid.Parse(exerciseLogId));
            return Ok(response);
        }
    }
}
