using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.Exercise;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;
using Fitness.Domain.Enums;

namespace Fitness.Application.Services
{
    public class ExerciseService(IExerciseRepository exerciseRepository, IMapper mapper) : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ExerciseResponseDto> CreateExerciseAsync(CreateExerciseDto createExerciseDto)
        {
            Exercise exercise = _mapper.Map<Exercise>(createExerciseDto);

            Exercise createdExercise = await _exerciseRepository.CreateAsync(exercise);

            return _mapper.Map<ExerciseResponseDto>(createdExercise);
        }

        public async Task<ExerciseResponseDto> UpdateExerciseAsync(UpdateExerciseDto updateExerciseDto, Guid exerciseId)
        {
            Exercise? updatedExercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (updatedExercise is null)
            {
                throw new NotFoundException(exerciseId, nameof(Exercise));
            }

            _mapper.Map(updateExerciseDto, updatedExercise);
            updatedExercise.UpdatedAt = DateTimeOffset.UtcNow;
            await _exerciseRepository.UpdateAsync(updatedExercise);

            return _mapper.Map<ExerciseResponseDto>(updatedExercise);
        }

        public async Task<DeleteResponseMessageDto> DeleteExerciseAsync(Guid exerciseId)
        {
            bool IsDeleted = await _exerciseRepository.DeleteAsync(exerciseId);

            if (!IsDeleted)
            {
                throw new NotFoundException(exerciseId, nameof(Exercise));
            }


            return new DeleteResponseMessageDto(exerciseId, nameof(Exercise));
        }

        public async Task<ICollection<ExerciseLightDto>> GetAllExercisesAsync(MuscleGroup? muscleGroup)
        {
            ICollection<Exercise> exercises = await _exerciseRepository.GetAllWithMuscleGroupAsync(muscleGroup);
            return _mapper.Map<ICollection<ExerciseLightDto>>(exercises);
        }

        public async Task<ExerciseResponseDto?> GetExerciseByIdAsync(Guid exerciseId)
        {
            Exercise? exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (exercise is null)
            {
                throw new NotFoundException(exerciseId, nameof(Exercise));
            }
            return _mapper.Map<ExerciseResponseDto>(exercise);
        }

        public async Task<List<MuscleGroup>> GetUniqueMuscleGroupsAsync()
        {
            return await _exerciseRepository.GetUniqueMuscleGroupsAsync();
        }
    }
}
