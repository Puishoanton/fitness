using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutSession;

namespace Fitness.Application.Interfaces.Services
{
    public interface IWorkoutSessionService
    {
        Task<WorkoutSessionResponseDto> CreateWorkoutSessionAsync(Guid userId, Guid workoutTemplateId);
        Task<WorkoutSessionResponseDto> UpdateWorkoutSessionAsync(Guid workoutSessionId, UpdateWorkoutSessionDto updateWorkoutSessionDto);
        Task<WorkoutSessionResponseDto> GetWorkoutSessionByIdAsync(Guid workoutSessionId);
        Task<List<WorkoutSessionLightDto>> GetAllWorkoutSessionsAsync();
        Task<DeleteResponseMessageDto> DeleteWorkoutSessionAsync(Guid workoutSessionId);
    }
}