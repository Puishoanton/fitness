using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplateExercise;

namespace Fitness.Application.Interfaces.Services
{
    public interface IWorkoutTemplateExerciseService
    {
        public Task<List<WorkoutTemplateExerciseResponseDto>> GetWorkoutTemplateExercisesByWorkoutTemplateIdAsync(Guid workoutTemplateId);
        public Task<WorkoutTemplateExerciseResponseDto> CreateWorkoutTemplateExerciseAsync(Guid workoutTemplateId, CreateWorkoutTemplateExerciseDto createWorkoutTemplateExerciseDto);
        public Task<DeleteResponseMessageDto> DeleteWorkoutTemplateExerciseAsync(Guid workoutTemplateExerciseId);
        public Task<WorkoutTemplateExerciseResponseDto> UpdateWorkoutTemplateExerciseAsync(Guid workoutTemplateExerciseId, UpdateWorkoutTemplateExerciseDto updateWorkoutTemplateExerciseDto);
        public Task MoveWorkoutTemplateExerciseAsync(MoveWorkoutTemplateExerciseDto moveWorkoutTemplateExerciseDto);
    }
}
