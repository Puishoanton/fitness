using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplateExercise;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class WorkoutTemplateExerciseService(IWorkoutTemplateExerciseRepository workoutTemplateExerciseRepository, IExerciseRepository exerciseRepository, IWorkoutTemplateRepository workoutTemplateRepository, IMapper mapper) : IWorkoutTemplateExerciseService
    {
        private readonly IWorkoutTemplateExerciseRepository _workoutTemplateExerciseRepository = workoutTemplateExerciseRepository;
        private readonly IWorkoutTemplateRepository _workoutTemplateRepository = workoutTemplateRepository;
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<WorkoutTemplateExerciseResponseDto>> GetWorkoutTemplateExercisesByWorkoutTemplateIdAsync(Guid workoutTemplateId)
        {
            List<WorkoutTemplateExercise> workoutTemplatesExercises = await _workoutTemplateExerciseRepository.GetAllByWorkoutTemplateIdAsync(workoutTemplateId);
            return _mapper.Map<List<WorkoutTemplateExerciseResponseDto>>(workoutTemplatesExercises.OrderBy(wte => wte.Order).ToList());
        }
        public async Task<WorkoutTemplateExerciseResponseDto> CreateWorkoutTemplateExerciseAsync(Guid workoutTemplateId, CreateWorkoutTemplateExerciseDto createWorkoutTemplateExerciseDto)
        {
            await AllowCreatingWorkoutTemplateExerciseAsync(workoutTemplateId, createWorkoutTemplateExerciseDto.ExerciseId);

            int count = await _workoutTemplateExerciseRepository.CountByWorkoutTemplateIdAsync(workoutTemplateId);
            WorkoutTemplateExercise workoutTemplateExercise = _mapper.Map<WorkoutTemplateExercise>(createWorkoutTemplateExerciseDto);
            workoutTemplateExercise.WorkoutTemplateId = workoutTemplateId;
            workoutTemplateExercise.Order = count + 1;

            await _workoutTemplateExerciseRepository.CreateAsync(workoutTemplateExercise);
            await _workoutTemplateExerciseRepository.SaveChangesAsync();

            return _mapper.Map<WorkoutTemplateExerciseResponseDto>(workoutTemplateExercise);
        }

        public async Task<DeleteResponseMessageDto> DeleteWorkoutTemplateExerciseAsync(Guid workoutTemplateExerciseId)
        {
            WorkoutTemplateExercise? workoutTemplateExercise = await _workoutTemplateExerciseRepository.GetByIdAsync(workoutTemplateExerciseId);
            if (workoutTemplateExercise is null)
            {
                throw new NotFoundException(workoutTemplateExerciseId, nameof(WorkoutTemplateExercise));
            }
            List<WorkoutTemplateExercise> workoutTemplateExercises = await _workoutTemplateExerciseRepository.GetAllByWorkoutTemplateIdAsync(workoutTemplateExercise.WorkoutTemplateId);

            int deletedOrder = workoutTemplateExercise.Order;

            await _workoutTemplateExerciseRepository.DeleteAsync(workoutTemplateExerciseId);

            foreach (WorkoutTemplateExercise item in workoutTemplateExercises.Where(wte => wte.Order > deletedOrder))
            {
                item.Order--;
            }

            await _workoutTemplateExerciseRepository.SaveChangesAsync();

            return new DeleteResponseMessageDto(workoutTemplateExerciseId, nameof(WorkoutTemplateExercise));
        }
        public async Task<WorkoutTemplateExerciseResponseDto> UpdateWorkoutTemplateExerciseAsync(Guid workoutTemplateExerciseId, UpdateWorkoutTemplateExerciseDto updateWorkoutTemplateExerciseDto)
        {
            WorkoutTemplateExercise? workoutTemplateExercise = await _workoutTemplateExerciseRepository.GetByIdAsync(workoutTemplateExerciseId);
            if (workoutTemplateExercise is null)
            {
                throw new NotFoundException(workoutTemplateExerciseId, nameof(WorkoutTemplateExercise));
            }

            _mapper.Map(updateWorkoutTemplateExerciseDto, workoutTemplateExercise);
            await _workoutTemplateExerciseRepository.UpdateAsync(workoutTemplateExercise);
            await _workoutTemplateExerciseRepository.SaveChangesAsync();

            return _mapper.Map<WorkoutTemplateExerciseResponseDto>(workoutTemplateExercise);
        }
        public async Task MoveWorkoutTemplateExerciseAsync(Guid workoutTemplateExerciseId, int newOrder)
        {
            WorkoutTemplateExercise? workoutTemplateExercise = await _workoutTemplateExerciseRepository.GetByIdAsync(workoutTemplateExerciseId);
            if (workoutTemplateExercise is null)
            {
                throw new NotFoundException(workoutTemplateExerciseId, nameof(WorkoutTemplateExercise));
            }


            List<WorkoutTemplateExercise> workoutTemplateExercises = await _workoutTemplateExerciseRepository.GetAllByWorkoutTemplateIdAsync(workoutTemplateExercise.WorkoutTemplateId);
            int oldOrder = workoutTemplateExercise.Order;
            newOrder = Math.Max(1, Math.Min(newOrder, workoutTemplateExercises.Count));


            if (newOrder == workoutTemplateExercise.Order) return;

            workoutTemplateExercise.Order = 0;
            await _workoutTemplateExerciseRepository.SaveChangesAsync();

            if (newOrder < oldOrder)
            {
                foreach (WorkoutTemplateExercise item in workoutTemplateExercises.Where(wte => wte.Order >= newOrder && wte.Order < oldOrder))
                {
                    item.Order++;
                }
            }
            else
            {
                foreach (WorkoutTemplateExercise item in workoutTemplateExercises.Where(wte => wte.Order > oldOrder && wte.Order <= newOrder))
                {
                    item.Order--;
                }
            }

            workoutTemplateExercise.Order = newOrder;
            await _workoutTemplateExerciseRepository.SaveChangesAsync();
        }
        private async Task AllowCreatingWorkoutTemplateExerciseAsync(Guid workoutTemplateId, Guid exerciseId)
        {
            WorkoutTemplate? workoutTemplate = await _workoutTemplateRepository.GetByIdAsync(workoutTemplateId);
            if (workoutTemplate is null)
            {
                throw new NotFoundException(workoutTemplateId, nameof(WorkoutTemplate));
            }

            Exercise? exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (exercise is null)
            {
                throw new NotFoundException(exerciseId, nameof(Exercise));
            }

            bool alreadyExists = await _workoutTemplateExerciseRepository
                 .ExistsAsync(wte => wte.WorkoutTemplateId == workoutTemplateId && wte.ExerciseId == exerciseId);

            if (alreadyExists)
                throw new BadRequestException($"Exercise {exerciseId} already exists in WorkoutTemplate {workoutTemplateId}.");
        }
    }
}
