using Fitness.Application.DTOs.Auth;
using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Interfaces.Services
{
    internal interface IAuthService
    {
        Task<AuthResponseDto> GoogleLoginAsync(string googleTokenId, HttpResponse response);
        Task<AuthMessageResponseDto> LogoutAsync(string refrshToken, HttpResponse response);
        Task<AuthResponseDto> RefreshTokenAsync(string refrshToken, HttpResponse response);
    }
}
