using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fitness.Application.DTOs.User;
using Fitness.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fitness.Application.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly SymmetricSecurityKey _accessTokenSigningKey = new(
            Encoding.UTF8.GetBytes(configuration["Jwt:AccessTokenSecretKey"] ??
                                   throw new InvalidOperationException("Jwt:AccessTokenSecretKey not found."))
        );
        private readonly SymmetricSecurityKey _refreshTokenSigningKey = new(
            Encoding.UTF8.GetBytes(configuration["Jwt:RefreshTokenSecretKey"] ??
                                   throw new InvalidOperationException("Jwt:RefreshTokenSecretKey not found."))
        );

        public string GenerateTokensAndSetToCookies(UserPayloadDto userPayloadDto, HttpResponse response)
        {
            Claim[] accessClaims = [
                new Claim(JwtRegisteredClaimNames.Sub, userPayloadDto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userPayloadDto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ];

            JwtSecurityToken accessToken = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: accessClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials( _accessTokenSigningKey, SecurityAlgorithms.HmacSha256)
                );
            JwtSecurityToken refreshToken = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(_refreshTokenSigningKey, SecurityAlgorithms.HmacSha256)
                );

            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            string refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            SetTokensToCookies(response, nameof(accessToken), accessTokenString, TimeSpan.FromMinutes(15));
            SetTokensToCookies(response, nameof(refreshToken), refreshTokenString, TimeSpan.FromDays(30));

            return refreshTokenString;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string? token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = _refreshTokenSigningKey
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        
        public void DeleteCookies(HttpResponse response)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(-1),
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            response.Cookies.Append("accessToken", string.Empty, cookieOptions);
            response.Cookies.Append("refreshToken", string.Empty, cookieOptions);
        }

        private static void SetTokensToCookies (HttpResponse response, string key, string value, TimeSpan maxAge)
        {
            CookieOptions cookieOptions = new ()
            {
                HttpOnly = true,
                Secure = true,
                MaxAge = maxAge,
                SameSite = SameSiteMode.None,
                Path = "/"
            };
            response.Cookies.Append(key, value, cookieOptions);
        }
    }
}
