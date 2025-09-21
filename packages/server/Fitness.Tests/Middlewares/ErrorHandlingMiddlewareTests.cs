using System.Text.Json;
using Fitness.Infrastructure.Middlewares;
using Fitness.Tests.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fitness.Tests.Middlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_WhenExpectedExceptionThrown_ReturnsExpectedProblemDetails()
        {
            //Arrange
            DefaultHttpContext httpContext = new();
            httpContext.Response.Body = new MemoryStream();
            Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();

            RequestDelegate next = _ => throw new TestApiException(StatusCodes.Status400BadRequest, "Something went wrong");

            ErrorHandlingMiddleware middleware = new(next, loggerMock.Object);

            //Act
            await middleware.InvokeAsync(httpContext);
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            string? body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            //Assert
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Should().NotBeNull();
            problem.Title.Should().Be(nameof(TestApiException));
            problem.Detail.Should().Be("Something went wrong");

        }

        [Fact]
        public async Task InvokeAsync_WhenUnexpectedExceptionThrown_ReturnsExpectedProblemDetails()
        {
            //Arrange
            DefaultHttpContext context = new();
            context.Response.Body = new MemoryStream();
            Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();

            RequestDelegate next = _ => throw new InvalidOperationException("Internal server error");
            ErrorHandlingMiddleware middleware = new(next, loggerMock.Object);

            //Act
            await middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string? body = await new StreamReader(context.Response.Body).ReadToEndAsync();

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Should().NotBeNull();
            problem!.Title.Should().Be("UnexpectedError");
        }

        [Fact]
        public async Task InvokeAsync_WhenUnauthorizedExceptionThrown_ReturnsExpectedProblemDetails()
        {
            //Arrange
            DefaultHttpContext context = new();
            context.Response.Body = new MemoryStream();
            Mock<ILogger<ErrorHandlingMiddleware>> loggerMock = new();

            RequestDelegate next = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            ErrorHandlingMiddleware middleware = new(next, loggerMock.Object);

            //Act
            await middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string? body = await new StreamReader(context.Response.Body).ReadToEndAsync();

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Should().NotBeNull();
            problem.Title.Should().Be("Unauthorized");
        }
    }
}
