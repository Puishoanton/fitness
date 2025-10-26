using AutoMapper;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class ExerciseLogService(IExerciseLogRepository exerciseLogRepository, IWorkoutSessionRepository workoutSessionRepository, IMapper mapper) : IExerciseLogService
    {
        private readonly IExerciseLogRepository _exerciseLogRepository = exerciseLogRepository;
        private readonly IWorkoutSessionRepository _workoutSessionRepository = workoutSessionRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ExerciseLogLightDto> CreateExerciseLogAsync(Guid exerciseId, Guid workoutSessionId)
        {
            WorkoutSession? workoutSession = await _workoutSessionRepository.GetByIdAsync(workoutSessionId);
            if (workoutSession is null)
            {
                throw new BadRequestException($"{nameof(WorkoutSession)}: {workoutSessionId} is not found.");
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
    }
}
