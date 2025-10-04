using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.WorkoutSession;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class WorkoutSessionService(IWorkoutSessionRepository workoutSessionRepository, IWorkoutTemplateRepository workoutTemplateRepository, IMapper mapper) : IWorkoutSessionService
    {
        private readonly IWorkoutSessionRepository _workoutSessionRepository = workoutSessionRepository;
        private readonly IWorkoutTemplateRepository _workoutTemplateRepository = workoutTemplateRepository;
        private readonly IMapper _mapper = mapper;


        public async Task<WorkoutSessionResponseDto> CreateWorkoutSessionAsync(Guid userId, Guid workoutTemplateId)
        {
            WorkoutTemplate? workoutTemplate = await _workoutTemplateRepository.GetByIdAsync(workoutTemplateId);
            if (workoutTemplate is null)
            {
                throw new BadRequestException($"{nameof(WorkoutTemplate)}: {workoutTemplateId} is not found.");
            }

            WorkoutSession workoutSession = new()
            {
                UserId = userId,
                WorkoutTemplateId = workoutTemplateId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _workoutSessionRepository.CreateAsync(workoutSession);

            return _mapper.Map<WorkoutSessionResponseDto>(workoutSession);
        }

        public async Task<WorkoutSessionResponseDto> UpdateWorkoutSessionAsync(Guid workoutSessionId, UpdateWorkoutSessionDto updateWorkoutSessionDto)
        {
            WorkoutSession? workoutSession = await _workoutSessionRepository.GetByIdAsync(workoutSessionId);
            if (workoutSession is null)
            {
                throw new NotFoundException(workoutSessionId, nameof(WorkoutSession));
            }
            _mapper.Map(updateWorkoutSessionDto, workoutSession);
            await _workoutSessionRepository.UpdateAsync(workoutSession);

            return _mapper.Map<WorkoutSessionResponseDto>(workoutSession);
        }

        public async Task<WorkoutSessionResponseDto> GetWorkoutSessionByIdAsync(Guid workoutSessionId)
        {
            WorkoutSession? workoutSession = await _workoutSessionRepository.GetByIdAsync(workoutSessionId);
            if (workoutSession is null)
            {
                throw new NotFoundException(workoutSessionId, nameof(WorkoutSession));
            }
            return _mapper.Map<WorkoutSessionResponseDto>(workoutSession);
        }

        public async Task<List<WorkoutSessionLightDto>> GetAllWorkoutSessionsAsync()
        {
            List<WorkoutSession> workoutSessions = await _workoutSessionRepository.GetAllListAsync();
            return _mapper.Map<List<WorkoutSessionLightDto>>(workoutSessions);
        }

        public async Task<DeleteResponseMessageDto> DeleteWorkoutSessionAsync(Guid workoutSessionId)
        {
            bool isDeleted = await _workoutSessionRepository.DeleteAsync(workoutSessionId);
            if (!isDeleted)
            {
                throw new NotFoundException(workoutSessionId, nameof(WorkoutSession));
            }
            return new(workoutSessionId, nameof(WorkoutSession));
        }
    }
}
