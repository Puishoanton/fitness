using Fitness.Application.DTOs.Common;
using Fitness.Application.DTOs.SetLog;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/set-logs")]
    public class SetLogController(ISetLogService setLogService) : ControllerBase
    {
        private readonly ISetLogService _setLogService = setLogService;

        [HttpGet]
        public async Task<IActionResult> GetAllSetLogs()
        {
            List<SetLogResponseDto> setLogs = await _setLogService.GetAllSetLogsAsync();
            return Ok(setLogs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSetLog([FromBody] CreateSetLogDto createSetLogDto, string exerciseLogId)
        {
            SetLogResponseDto setLog = await _setLogService.CreateSetLogAsync(createSetLogDto, Guid.Parse(exerciseLogId));
            return Ok(setLog);
        }

        [HttpPatch("{setLogId}")]
        public async Task<IActionResult> UpdateSetLog(string setLogId, [FromBody] UpdateSetLogDto updateSetLogDto)
        {
            SetLogResponseDto updatedSetLog = await _setLogService.UpdateSetLogAsync(Guid.Parse(setLogId), updateSetLogDto);
            return Ok(updatedSetLog);
        }

        [HttpDelete("{setLogId}")]
        public async Task<IActionResult> DeleteSetLog(string setLogId)
        {
            DeleteResponseMessageDto response = await _setLogService.DeleteSetLogAsync(Guid.Parse(setLogId));
            return Ok(response);
        }
    }
}
