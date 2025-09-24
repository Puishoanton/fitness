using AutoMapper;
using Fitness.Application.DTOs.Auth;
using Fitness.Application.DTOs.User;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Application.Interfaces.Validators;
using Fitness.Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Fitness.Application.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IGoogleTokenValidator googleTokenValidator, IMapper mapper) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IGoogleTokenValidator _googleTokenValidator = googleTokenValidator;
        private readonly IMapper _mapper = mapper;
        public async Task<AuthResponseDto> GoogleLoginAsync(string googleTokenId, HttpResponse response)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await _googleTokenValidator.ValidateAsync(googleTokenId);
            }
            catch (InvalidJwtException)
            {
                throw new BadRequestException("Invalid Google ID token.");
            }

            User? user = await _userRepository.GetUserByEmailAsync(payload.Email);
            if (user == null)
            {
                CreateUserDto createUserDto = new() { Email = payload.Email };
                user = _mapper.Map<User>(createUserDto);

                await _userRepository.CreateAsync(user);
            }

            UserPayloadDto userPayloadDto = _mapper.Map<UserPayloadDto>(user);

            user.RefreshToken = _tokenService.GenerateTokensAndSetToCookies(userPayloadDto, response);
            await _userRepository.UpdateAsync(user);

            return _mapper.Map<AuthResponseDto>(user);
        }

        public async Task<AuthMessageResponseDto> LogoutAsync(string refreshToken, HttpResponse response)
        {
            try
            {
                _tokenService.GetPrincipalFromToken(refreshToken);
            }
            catch (SecurityTokenException)
            {
                return new AuthMessageResponseDto() { Message = "Token is invalid" };
            }
            User? user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user is not null && user.RefreshToken == refreshToken)
            {
                user.RefreshToken = string.Empty;
                await _userRepository.UpdateAsync(user);
            }

            _tokenService.DeleteCookies(response);

            return new AuthMessageResponseDto() { Message = "Logout success" };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string? refreshToken, HttpResponse response)
        {
            try
            {
                _tokenService.GetPrincipalFromToken(refreshToken);
            }
            catch
            {
                throw new BadRequestException("Invalid refresh token.");
            }
            User? user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user is null || user.RefreshToken != refreshToken)
            {
                if (user is not null)
                {
                    user.RefreshToken = string.Empty;
                    await _userRepository.UpdateAsync(user);
                }

                throw new BadRequestException("Invalid refresh token.");
            }
            UserPayloadDto userPayloadDto = _mapper.Map<UserPayloadDto>(user);

            user.RefreshToken = _tokenService.GenerateTokensAndSetToCookies(userPayloadDto, response);
            await _userRepository.UpdateAsync(user);

            return _mapper.Map<AuthResponseDto>(user);
        }
    }
}
