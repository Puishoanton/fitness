using Fitness.Application.DTOs.ExerciseLog;

namespace Fitness.Application.Interfaces.Services
{
    public interface IExerciseLogService
    {
        Task<ExerciseLogLightDto> CreateExerciseLogAsync(Guid exerciseId, Guid workoutSessionId);
        Task<ExerciseLogLightDto> UpdateExerciseLogAsync(Guid exerciseLogId, UpdateExerciseLogDto updateExerciseLogDto);
        Task<ExerciseLogLightDto> GetExerciseLogByIdAsync(Guid exerciseLogId);
        Task<List<ExerciseLogLightDto>> GetAllExerciseLogsAsync();
    }
}
