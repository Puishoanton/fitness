using AutoMapper;
using Fitness.Application.DTOs.WorkoutTemplate;
using Fitness.Application.Exceptions.WorkoutTemplate;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class WorkoutTemplateService(IWorkoutTemplateRepository workoutTemplateRepository, IMapper mapper) : IWorkoutTemplateService
    {
        private readonly IWorkoutTemplateRepository _workoutTemplateRepository = workoutTemplateRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<WorkoutTemplateResponseDto> CreateWorkoutTemplateAsync(CreateWorkoutTemplateDto createWorkoutTemplateDto, Guid userId)
        {
            WorkoutTemplate workoutTemplate = _mapper.Map<WorkoutTemplate>(createWorkoutTemplateDto);

            workoutTemplate.UserId = userId;
            WorkoutTemplate createdWorkoutTemplate = await _workoutTemplateRepository.CreateAsync(workoutTemplate);

            return _mapper.Map<WorkoutTemplateResponseDto>(createdWorkoutTemplate);
        }

        public async Task<WorkoutTemplateResponseDto> UpdateWorkoutTemplateAsync(UpdateWorkoutTemplateDto updateWorkoutTemplateDto, Guid workoutTemplateId)
        {
            WorkoutTemplate? updatedWorkoutTemplate = await _workoutTemplateRepository.GetByIdAsync(workoutTemplateId);
            if (updatedWorkoutTemplate is null)
            {
                throw new WorkoutTemplateNotFoundException(workoutTemplateId);
            }

            _mapper.Map(updateWorkoutTemplateDto, updatedWorkoutTemplate);
            await _workoutTemplateRepository.UpdateAsync(updatedWorkoutTemplate);

            return _mapper.Map<WorkoutTemplateResponseDto>(updatedWorkoutTemplate);
        }

        public async Task<WorkoutTemplateResponseMessageDto> DeleteWorkoutTemplateAsync(Guid workoutTemplateId)
        {
            bool IsDeleted = await _workoutTemplateRepository.DeleteAsync(workoutTemplateId);

            if (!IsDeleted)
            {
                throw new WorkoutTemplateNotFoundException(workoutTemplateId);
            }


            return new WorkoutTemplateResponseMessageDto() { Message = $"WorkoutTemplateId: {workoutTemplateId} is deleted." };
        }

        public async Task<WorkoutTemplateWithExercisesDto?> GetWorkoutTemplateByIdWithExercisesAsync(Guid workoutTemplateId)
        {
            WorkoutTemplate? workoutTemplate = await _workoutTemplateRepository.GetWorkoutTemplateByIdWithExercises(workoutTemplateId);
            if (workoutTemplate is null)
            {
                throw new WorkoutTemplateNotFoundException(workoutTemplateId);
            }

            return _mapper.Map<WorkoutTemplateWithExercisesDto>(workoutTemplate);
        }

        public async Task<ICollection<WorkoutTemplateResponseDto>> GetWorkoutTemplatesAsync()
        {
            ICollection<WorkoutTemplate> workoutTemplates = await _workoutTemplateRepository.GetAllAsync();
            return _mapper.Map<ICollection<WorkoutTemplateResponseDto>>(workoutTemplates);
        }
    }
}
