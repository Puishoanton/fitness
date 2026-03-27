using AutoMapper;
using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.SetLog;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Domain.Entities;

namespace Fitness.Application.Services
{
    public class SetLogService(ISetLogRepository setLogRepository, IExerciseLogRepository exerciseLogRepository, IMapper mapper) : ISetLogService
    {
        public readonly ISetLogRepository _setLogRepository = setLogRepository;
        public readonly IExerciseLogRepository _exerciseLogRepository = exerciseLogRepository;
        public readonly IMapper _mapper = mapper;

        public async Task<SetLogResponseDto> CreateSetLogAsync(CreateSetLogDto createSetLogDto, Guid exerciseLogId)
        {
            ExerciseLog? exerciseLog = await _exerciseLogRepository.GetByIdAsync(exerciseLogId);
            if (exerciseLog is null)
            {
                throw new BadRequestException($"{nameof(ExerciseLog)}: {exerciseLogId} is not found.");
            }
            int existingSetLogsCount = await _setLogRepository.CountByExerciseLogIdAsync(exerciseLogId);
            SetLog setLog = _mapper.Map<SetLog>(createSetLogDto);
            setLog.Order = existingSetLogsCount + 1;
            setLog.ExerciseLogId = exerciseLogId;
            SetLog createdSetLog = await _setLogRepository.CreateAsync(setLog);

            return _mapper.Map<SetLogResponseDto>(createdSetLog);
        }
        public async Task<SetLogResponseDto> UpdateSetLogAsync(Guid setLogId, UpdateSetLogDto updateSetLogDto)
        {
            SetLog? existingSetLog = await _setLogRepository.GetByIdAsync(setLogId);
            if (existingSetLog is null)
            {
                throw new BadRequestException($"{nameof(SetLog)}: {setLogId} is not found.");
            }

            _mapper.Map(updateSetLogDto, existingSetLog);
            SetLog updatedSetLog = await _setLogRepository.UpdateAsync(existingSetLog);

            return _mapper.Map<SetLogResponseDto>(updatedSetLog);
        }
        public async Task<SetLogResponseDto> GetSetLogByIdAsync(Guid setLogId)
        {
            SetLog? setLog = await _setLogRepository.GetByIdAsync(setLogId);
            if (setLog is null)
            {
                throw new NotFoundException(setLogId, nameof(SetLog));
            }

            return _mapper.Map<SetLogResponseDto>(setLog);
        }
        public async Task<List<SetLogResponseDto>> GetAllSetLogsAsync()
        {
            List<SetLog> setLogs = await _setLogRepository.GetAllListAsync();
            return _mapper.Map<List<SetLogResponseDto>>(setLogs);
        }
        public async Task<DeleteResponseMessageDto> DeleteSetLogAsync(Guid setLogId)
        {
            bool isDeleted = await _setLogRepository.DeleteAsync(setLogId);
            if (!isDeleted)
            {
                throw new NotFoundException(setLogId, nameof(SetLog));
            }
            return new DeleteResponseMessageDto(setLogId, nameof(SetLog));
        }
    }
}
