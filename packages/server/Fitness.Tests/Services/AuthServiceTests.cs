using System.Security.Claims;
using AutoMapper;
using Fitness.Application.DTOs.Auth;
using Fitness.Application.DTOs.User;
using Fitness.Application.Exceptions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Application.Interfaces.Validators;
using Fitness.Application.Services;
using Fitness.Domain.Entities;
using FluentAssertions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Fitness.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task GoogleLoginAsync_WhenUserExist_ShouldCreateRefreshTokenSetItToCookiesAndReturnAuthResponseDto()
        {
            // Arrange
            const string userEmail = "user_email@test.com";
            const string googleTokenId = "fake_google_token_id";
            const string refreshToken = "fake_refresh_token";


            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            GoogleJsonWebSignature.Payload payload = new() { Email = userEmail };
            googleTokenValidatorMock
                .Setup(v => v.ValidateAsync(googleTokenId))
                .ReturnsAsync(payload);

            User existingUser = new() { Email = userEmail };
            userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(payload.Email))
                .ReturnsAsync(existingUser);

            UserPayloadDto userPayloadDto = new() { Email = userEmail };
            mapperMock
                .Setup(m => m.Map<UserPayloadDto>(It.IsAny<User>()))
                .Returns(userPayloadDto);

            tokenServiceMock
                .Setup(s => s.GenerateTokensAndSetToCookies(userPayloadDto, response))
                .Returns(refreshToken);

            AuthResponseDto authResponseDto = new() { Email = userEmail };
            mapperMock
                .Setup(m => m.Map<AuthResponseDto>(existingUser))
                .Returns(authResponseDto);

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            // Act
            AuthResponseDto? result = await authService.GoogleLoginAsync(googleTokenId, response);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(userEmail);

            googleTokenValidatorMock.Verify(v => v.ValidateAsync(googleTokenId), Times.Once);
            userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.RefreshToken == refreshToken)), Times.Once);
        }

        [Fact]
        public async Task GoogleLoginAsync_WhenUserNotExist_ShouldCreateRefreshTokenSetItToCookiesAndReturnAuthResponseDto()
        {
            // Arrange
            const string userEmail = "user_email@test.com";
            const string googleTokenId = "fake_google_token_id";
            const string refreshToken = "fake_refresh_token";


            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            GoogleJsonWebSignature.Payload payload = new() { Email = userEmail };
            googleTokenValidatorMock
                .Setup(v => v.ValidateAsync(googleTokenId))
                .ReturnsAsync(payload);

            userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(payload.Email))
                .ReturnsAsync((User?)null);

            CreateUserDto createUserDto = new() { Email = payload.Email };
            User newUser = new() { Email = createUserDto.Email };
            mapperMock
                .Setup(m => m.Map<User>(It.IsAny<CreateUserDto>()))
                .Returns(newUser);

            UserPayloadDto userPayloadDto = new() { Email = newUser.Email };
            mapperMock
                .Setup(m => m.Map<UserPayloadDto>(It.IsAny<User>()))
                .Returns(userPayloadDto);

            tokenServiceMock
                .Setup(s => s.GenerateTokensAndSetToCookies(userPayloadDto, response))
                .Returns(refreshToken);

            AuthResponseDto authResponseDto = new() { Email = userEmail };
            mapperMock
                .Setup(m => m.Map<AuthResponseDto>(It.IsAny<User>()))
                .Returns(authResponseDto);

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            // Act
            AuthResponseDto? result = await authService.GoogleLoginAsync(googleTokenId, response);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(userEmail);

            googleTokenValidatorMock.Verify(v => v.ValidateAsync(googleTokenId), Times.Once);
            userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.RefreshToken == refreshToken)), Times.Once);
        }

        [Fact]
        public async Task GoogleLoginAsync_WhenGoogleTokenIdIsNotValid_ShouldThrowBadRequestException()
        {
            // Arrange
            const string googleTokenId = "fake_google_token_id";


            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            googleTokenValidatorMock
                .Setup(v => v.ValidateAsync(googleTokenId))
                .ThrowsAsync(new InvalidJwtException("Invalid Google ID token."));

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            // Act
            Func<Task> act = async () => await authService.GoogleLoginAsync(googleTokenId, response);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Invalid Google ID token.");

            googleTokenValidatorMock.Verify(v => v.ValidateAsync(googleTokenId), Times.Once);
            userRepositoryMock.Verify(r => r.GetUserByEmailAsync(It.IsAny<string>()), Times.Never);
            tokenServiceMock.Verify(s => s.GenerateTokensAndSetToCookies(It.IsAny<UserPayloadDto>(), response), Times.Never);
            mapperMock.Verify(m => m.Map<User>(It.IsAny<CreateUserDto>()), Times.Never);
        }

        [Fact]
        public async Task LogoutAsync_WhenRefreshTokenIsValid_ShouldReturnLogoutSuccess()
        {
            const string refreshToken = "fake_refresh_token";

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            ClaimsPrincipal principal = new();
            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(refreshToken))
                .Returns(principal);

            User user = new() { RefreshToken = refreshToken };
            userRepositoryMock
                .Setup(r => r.GetUserByRefreshTokenAsync(refreshToken))
                .ReturnsAsync(user);


            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            AuthMessageResponseDto result = await authService.LogoutAsync(refreshToken, response);

            //Assert
            result.Message.Should().Be("Logout success");

            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.RefreshToken == string.Empty)), Times.Once);
            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(refreshToken), Times.Once);
            tokenServiceMock.Verify(s => s.DeleteCookies(response), Times.Once);
        }

        [Fact]
        public async Task LogoutAsync_WhenRefreshTokenIsNotValid_ShouldReturnTokenIsInvalid()
        {
            const string refreshToken = "";

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            ClaimsPrincipal principal = new();
            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(refreshToken))
                .Throws(new SecurityTokenException("Invalid token"));


            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            AuthMessageResponseDto result = await authService.LogoutAsync(refreshToken, response);

            //Assert
            result.Message.Should().Be("Token is invalid");

            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(refreshToken), Times.Never);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.RefreshToken == string.Empty)), Times.Never);
            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(refreshToken), Times.Once);
            tokenServiceMock.Verify(s => s.DeleteCookies(response), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenIsNotValid_ShouldReturnTokenIsInvalid()
        {
            string refreshToken = string.Empty;

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(refreshToken))
                .Throws(new BadRequestException("Invalid refresh token."));

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await authService.RefreshTokenAsync(refreshToken, response);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Invalid refresh token.");

            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(refreshToken), Times.Never);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
            mapperMock.Verify(m => m.Map<UserPayloadDto>(It.IsAny<User>()), Times.Never);
            tokenServiceMock.Verify(s => s.GenerateTokensAndSetToCookies(It.IsAny<UserPayloadDto>(), response), Times.Never);
            mapperMock.Verify(m => m.Map<AuthResponseDto>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserWithSuchRefreshTokenNotFound_ShouldReturnTokenIsInvalid()
        {
            string refreshToken = string.Empty;

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(refreshToken))
                .Returns(new ClaimsPrincipal());

            userRepositoryMock
                .Setup(r => r.GetUserByRefreshTokenAsync(refreshToken))
                .ReturnsAsync((User?)null);

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await authService.RefreshTokenAsync(refreshToken, response);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Invalid refresh token.");

            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
            mapperMock.Verify(m => m.Map<UserPayloadDto>(It.IsAny<User>()), Times.Never);
            tokenServiceMock.Verify(s => s.GenerateTokensAndSetToCookies(It.IsAny<UserPayloadDto>(), response), Times.Never);
            mapperMock.Verify(m => m.Map<AuthResponseDto>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserRefreshTokenNotEqualWithReqestRefreshToken_ShouldReturnTokenIsInvalid()
        {
            const string requestRefreshToken = "request_refresh_token";
            const string userRefreshToken = "user_refresh_token";

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(requestRefreshToken))
                .Returns(new ClaimsPrincipal());

            userRepositoryMock
                .Setup(r => r.GetUserByRefreshTokenAsync(requestRefreshToken))
                .ReturnsAsync(new User() { RefreshToken = userRefreshToken });

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            Func<Task> act = async () => await authService.RefreshTokenAsync(requestRefreshToken, response);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Invalid refresh token.");

            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(requestRefreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(requestRefreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
            mapperMock.Verify(m => m.Map<UserPayloadDto>(It.IsAny<User>()), Times.Never);
            tokenServiceMock.Verify(s => s.GenerateTokensAndSetToCookies(It.IsAny<UserPayloadDto>(), response), Times.Never);
            mapperMock.Verify(m => m.Map<AuthResponseDto>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenIsValid_ShouldRefreshToken()
        {
            const string refreshToken = "refresh_token";
            const string userEmail = "user_email@test.com";

            //Arrange
            DefaultHttpContext httpContext = new();
            HttpResponse response = httpContext.Response;

            Mock<IUserRepository> userRepositoryMock = new();
            Mock<ITokenService> tokenServiceMock = new();
            Mock<IGoogleTokenValidator> googleTokenValidatorMock = new();
            Mock<IMapper> mapperMock = new();

            tokenServiceMock
                .Setup(s => s.GetPrincipalFromToken(refreshToken))
                .Returns(new ClaimsPrincipal());

            User user = new() { Email = userEmail, RefreshToken = refreshToken };
            userRepositoryMock
                .Setup(r => r.GetUserByRefreshTokenAsync(refreshToken))
                .ReturnsAsync(user);

            UserPayloadDto userPayloadDto = new();
            mapperMock
                .Setup(m => m.Map<UserPayloadDto>(It.IsAny<User>()))
                .Returns(userPayloadDto);

            const string newRefreshToken = "new_refresh_token";
            tokenServiceMock
                .Setup(s => s.GenerateTokensAndSetToCookies(userPayloadDto, response))
                .Returns(newRefreshToken);

            User updatedUser = new()
            {
                Email = userEmail,
                RefreshToken = newRefreshToken
            };
            userRepositoryMock
                .Setup(r => r.UpdateAsync(user))
                .ReturnsAsync(updatedUser);

            mapperMock
                .Setup(m => m.Map<AuthResponseDto>(It.IsAny<User>()))
                .Returns(new AuthResponseDto() { Email = updatedUser.Email });

            AuthService authService = new(
                userRepositoryMock.Object,
                tokenServiceMock.Object,
                googleTokenValidatorMock.Object,
                mapperMock.Object
                );

            //Act
            AuthResponseDto result = await authService.RefreshTokenAsync(refreshToken, response);

            //Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(userEmail);

            tokenServiceMock.Verify(s => s.GetPrincipalFromToken(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.GetUserByRefreshTokenAsync(refreshToken), Times.Once);
            userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
            mapperMock.Verify(m => m.Map<UserPayloadDto>(It.IsAny<User>()), Times.Once);
            tokenServiceMock.Verify(s => s.GenerateTokensAndSetToCookies(It.IsAny<UserPayloadDto>(), response), Times.Once);
            mapperMock.Verify(m => m.Map<AuthResponseDto>(It.IsAny<User>()), Times.Once);
        }
    }
}
