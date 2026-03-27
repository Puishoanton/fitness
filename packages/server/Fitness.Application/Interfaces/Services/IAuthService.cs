using Fitness.Application.DTOs.Auth;
using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> GoogleLoginAsync(string googleTokenId, HttpResponse response);
        Task<AuthMessageResponseDto> LogoutAsync(string refrshToken, HttpResponse response);
        Task<AuthResponseDto> RefreshTokensAsync(string? refrshToken, HttpResponse response);
    }
}
