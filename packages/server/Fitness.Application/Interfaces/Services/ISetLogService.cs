using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Application.DTOs.SetLog;
using Fitness.Application.DTOs.WorkoutSession;

namespace Fitness.Application.Interfaces.Services
{
    public interface ISetLogService
    {
        public Task<SetLogResponseDto> CreateSetLogAsync(CreateSetLogDto createSetLogDto, Guid exerciseLogId);
        public Task<SetLogResponseDto> UpdateSetLogAsync(Guid setLogId, UpdateSetLogDto updateSetLogDto);
        public Task<SetLogResponseDto> GetSetLogByIdAsync(Guid setLogId);
        public Task<List<SetLogResponseDto>> GetAllSetLogsAsync();
        public Task<DeleteResponseMessageDto> DeleteSetLogAsync(Guid setLogId);

    }
}
