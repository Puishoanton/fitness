using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplate;

namespace Fitness.Application.Interfaces.Services
{
    public interface IWorkoutTemplateService
    {
        public Task<WorkoutTemplateResponseDto> CreateWorkoutTemplateAsync(CreateWorkoutTemplateDto createWorkoutTemplateDto, Guid userId);
        public Task<WorkoutTemplateResponseDto> UpdateWorkoutTemplateAsync(UpdateWorkoutTemplateDto updateWorkoutTemplateDto, Guid workoutTemplateId);
        public Task<DeleteResponseMessageDto> DeleteWorkoutTemplateAsync(Guid workoutTemplateId);
        public Task<WorkoutTemplateWithExercisesDto?> GetWorkoutTemplateByIdWithExercisesAsync(Guid workoutTemplateId);
        public Task<ICollection<WorkoutTemplateResponseDto>> GetWorkoutTemplatesAsync();
    }
}
