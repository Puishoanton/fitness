using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Application.DTOs.WorkoutTemplateExercise;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class ExerciseLogService(IExerciseLogRepository exerciseLogRepository, IWorkoutSessionRepository workoutSessionRepository, IExerciseRepository exerciseRepository, IWorkoutTemplateExerciseService workoutTemplateExerciseService, IMapper mapper) : IExerciseLogService
    {
        private readonly IExerciseLogRepository _exerciseLogRepository = exerciseLogRepository;
        private readonly IWorkoutSessionRepository _workoutSessionRepository = workoutSessionRepository;
        private readonly IExerciseRepository _exerciseRepository = exerciseRepository;
        private readonly IWorkoutTemplateExerciseService _workoutTemplateExerciseService = workoutTemplateExerciseService;
        private readonly IMapper _mapper = mapper;
        public async Task<ExerciseLogLightDto> CreateExerciseLogAsync(Guid exerciseId, Guid workoutSessionId)
        {
            WorkoutSession? workoutSession = await _workoutSessionRepository.GetByIdAsync(workoutSessionId);
            if (workoutSession is null)
            {
                throw new BadRequestException($"{nameof(WorkoutSession)}: {workoutSessionId} is not found.");
            }
            Exercise? exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if (exercise is null)
            {
                throw new NotFoundException(exerciseId, nameof(Exercise));
            }

            int order = await _exerciseLogRepository.CountByWorkoutSessionIdAsync(workoutSessionId);

            ExerciseLog exerciseLog = new()
            {
                ExerciseId = exerciseId,
                WorkoutSessionId = workoutSessionId,
                Order = order + 1,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            exerciseLog = await _exerciseLogRepository.CreateAsync(exerciseLog);

            CreateWorkoutTemplateExerciseDto createWte = new()
            {
                ExerciseId = exerciseId
            };
            await _workoutTemplateExerciseService.CreateWorkoutTemplateExerciseAsync(workoutSession.WorkoutTemplateId, createWte);

            return _mapper.Map<ExerciseLogLightDto>(exerciseLog);
        }
        public async Task<ExerciseLogLightDto> UpdateExerciseLogAsync(Guid exerciseLogId, UpdateExerciseLogDto updateExerciseLogDto)
        {
            ExerciseLog? exerciseLog = await _exerciseLogRepository.GetByIdAsync(exerciseLogId);
            if (exerciseLog is null)
            {
                throw new NotFoundException(exerciseLogId, nameof(ExerciseLog));
            }

            _mapper.Map(updateExerciseLogDto, exerciseLog);
            exerciseLog.UpdatedAt = DateTimeOffset.UtcNow;
            await _exerciseLogRepository.UpdateAsync(exerciseLog);

            return _mapper.Map<ExerciseLogLightDto>(exerciseLog);
        }
        public async Task<ExerciseLogLightDto> GetExerciseLogByIdAsync(Guid exerciseLogId)
        {
            ExerciseLog? exerciseLog = await _exerciseLogRepository.GetByIdAsync(exerciseLogId);
            if (exerciseLog is null)
            {
                throw new NotFoundException(exerciseLogId, nameof(ExerciseLog));
            }

            return _mapper.Map<ExerciseLogLightDto>(exerciseLog);
        }
        public async Task<List<ExerciseLogLightDto>> GetAllExerciseLogsAsync()
        {
            List<ExerciseLog> exerciseLogs = await _exerciseLogRepository.GetAllListAsync();
            return _mapper.Map<List<ExerciseLogLightDto>>(exerciseLogs);
        }
        public async Task<DeleteResponseMessageDto> DeleteExerciseLogAsync(Guid exerciseLogId)
        {
            bool IsDeleted = await _exerciseLogRepository.DeleteAsync(exerciseLogId);

            if (!IsDeleted)
            {
                throw new NotFoundException(exerciseLogId, nameof(ExerciseLog));
            }


            return new DeleteResponseMessageDto(exerciseLogId, nameof(ExerciseLog));
        }
        public List<ExerciseLog> CreateExerciseLogsFromWorkoutTemplateExercises(List<WorkoutTemplateExercise> workoutTemplateExercises)
        {
            return workoutTemplateExercises
                .OrderBy(wte => wte.Order)
                .Select(wte => new ExerciseLog
                {
                    WorkoutTemplateExerciseId = wte.Id,
                    ExerciseId = wte.ExerciseId,
                    SetLogs = [],
                    Order = wte.Order
                })
                .ToList();
        }
    }
}
