using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Services
{
    public interface IExerciseLogService
    {
        public Task<ExerciseLogLightDto> CreateExerciseLogAsync(Guid exerciseId, Guid workoutSessionId);
        public Task<ExerciseLogLightDto> GetExerciseLogByIdAsync(Guid exerciseLogId);
        public Task<List<ExerciseLogLightDto>> GetAllExerciseLogsAsync();
        public Task<DeleteResponseMessageDto> DeleteExerciseLogAsync(Guid exerciseLogId);
        public List<ExerciseLog> CreateExerciseLogsFromWorkoutTemplateExercises(List<WorkoutTemplateExercise> workoutTemplateExercises);

    }
}
