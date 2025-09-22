using System.Security.Claims;
using Fitness.Application.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateTokensAndSetToCookies(UserPayloadDto userPayloadDto, HttpResponse response);
        public ClaimsPrincipal GetPrincipalFromToken(string? token);
        public void DeleteCookies(HttpResponse response);
    }
}
