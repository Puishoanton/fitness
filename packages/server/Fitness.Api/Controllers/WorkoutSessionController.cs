using System.Security.Claims;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutSession;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/workout-sessions")]
    public class WorkoutSessionController(IWorkoutSessionService workoutSessionService) : ControllerBase
    {
        private readonly IWorkoutSessionService _workoutSessionService = workoutSessionService;

        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutSessions()
        {
            List<WorkoutSessionLightDto> workoutSessions = await _workoutSessionService.GetAllWorkoutSessionsAsync();
            return Ok(workoutSessions);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutSession(string workoutTemplateId)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            WorkoutSessionResponseDto workoutSessionResponse = await _workoutSessionService.CreateWorkoutSessionAsync(userId, Guid.Parse(workoutTemplateId));
            return Ok(workoutSessionResponse);
        }

        [HttpPatch("{workoutSessionId}")]
        public async Task<IActionResult> UpdateWorkoutSession(string workoutSessionId, [FromBody] UpdateWorkoutSessionDto updateWorkoutSessionDto)
        {
            WorkoutSessionResponseDto workoutSessionResponse = await _workoutSessionService.UpdateWorkoutSessionAsync(Guid.Parse(workoutSessionId), updateWorkoutSessionDto);
            return Ok(workoutSessionResponse);
        }

        [HttpGet("{workoutSessionId}")]
        public async Task<IActionResult> GetWorkoutSessionById(string workoutSessionId)
        {
            WorkoutSessionResponseDto workoutSessionResponse = await _workoutSessionService.GetWorkoutSessionByIdAsync(Guid.Parse(workoutSessionId));
            return Ok(workoutSessionResponse);
        }

        [HttpDelete("{workoutSessionId}")]
        public async Task<IActionResult> DeleteWorkoutSession(string workoutSessionId)
        {
            DeleteResponseMessageDto deleteResponse = await _workoutSessionService.DeleteWorkoutSessionAsync(Guid.Parse(workoutSessionId));
            return Ok(deleteResponse);
        }
    }
}
