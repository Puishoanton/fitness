using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutTemplateExercise;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class WorkoutTemplateExerciseService(IWorkoutTemplateExerciseRepository workoutTemplateExerciseRepository, IWorkoutTemplateRepository workoutTemplateRepository, IMapper mapper) : IWorkoutTemplateExerciseService
    {
        private readonly IWorkoutTemplateExerciseRepository _workoutTemplateExerciseRepository = workoutTemplateExerciseRepository;
        private readonly IWorkoutTemplateRepository _workoutTemplateRepository = workoutTemplateRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<WorkoutTemplateExerciseResponseDto>> GetWorkoutTemplateExercisesByWorkoutTemplateIdAsync(Guid workoutTemplateId)
        {
            List<WorkoutTemplateExercise> workoutTemplatesExercises = await _workoutTemplateExerciseRepository.GetAllByWorkoutTemplateIdAsync(workoutTemplateId);
            return _mapper.Map<List<WorkoutTemplateExerciseResponseDto>>(workoutTemplatesExercises.OrderBy(wte => wte.Order).ToList());
        }

        public async Task<WorkoutTemplateExerciseResponseDto> CreateWorkoutTemplateExerciseAsync(Guid workoutTemplateId, CreateWorkoutTemplateExerciseDto createWorkoutTemplateExerciseDto)
        {
            WorkoutTemplate? workoutTemplate = await _workoutTemplateRepository.GetByIdAsync(workoutTemplateId);
            if (workoutTemplate is null)
            {
                throw new NotFoundException(workoutTemplateId, nameof(WorkoutTemplate));
            }

            int count = await _workoutTemplateExerciseRepository.CountByWorkoutTemplateIdAsync(workoutTemplateId);
            WorkoutTemplateExercise workoutTemplateExercise = _mapper.Map<WorkoutTemplateExercise>(createWorkoutTemplateExerciseDto);
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

        public async Task MoveWorkoutTemplateExerciseAsync(MoveWorkoutTemplateExerciseDto moveWorkoutTemplateExerciseDto)
        {
            WorkoutTemplateExercise? workoutTemplateExercise = await _workoutTemplateExerciseRepository.GetByIdAsync(moveWorkoutTemplateExerciseDto.Id);
            if (workoutTemplateExercise is null)
            {
                throw new NotFoundException(moveWorkoutTemplateExerciseDto.Id, nameof(WorkoutTemplateExercise));
            }
            if (moveWorkoutTemplateExerciseDto.NewOrder == workoutTemplateExercise.Order) return;

            List<WorkoutTemplateExercise> workoutTemplateExercises = await _workoutTemplateExerciseRepository.GetAllByWorkoutTemplateIdAsync(workoutTemplateExercise.WorkoutTemplateId);
            int newOrder = moveWorkoutTemplateExerciseDto.NewOrder;
            int oldOrder = workoutTemplateExercise.Order;

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
    }
}
