using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.Exercise;

namespace Fitness.Application.Interfaces.Services
{
    public interface IExerciseService
    {
        public Task<ExerciseResponseDto> CreateExerciseAsync(CreateExerciseDto createExerciseDto);
        public Task<ExerciseResponseDto> UpdateExerciseAsync(UpdateExerciseDto updateExerciseDto, Guid exerciseId);
        public Task<DeleteResponseMessageDto> DeleteExerciseAsync(Guid exerciseId);
        public Task<ICollection<ExerciseLightDto>> GetAllExercisesAsync();
        public Task<ExerciseResponseDto?> GetExerciseByIdAsync(Guid exerciseId);

    }
}
