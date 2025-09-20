using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fitness.Application.Exceptions;

namespace Fitness.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Response.ContentType = "application/json";

                    ProblemDetails problem = new()
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Detail = "Access token is required",
                        Instance = context.Request.Path
                    };

                    string? json = JsonSerializer.Serialize(problem);
                    await context.Response.WriteAsync(json);
                }
            }
            catch (ApiException ex)
            {
                await HandleExpectedExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleUnexpectedExceptionAsync(context);
            }
        }

        public static Task HandleExpectedExceptionAsync(HttpContext context, ApiException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            ProblemDetails problem = new()
            {
                Status = ex.StatusCode,
                Title = ex.GetType().Name,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;

            string? json = JsonSerializer.Serialize(problem);
            return context.Response.WriteAsync(json);
        }

        public static Task HandleUnexpectedExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problem = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "UnexpectedError",
                Detail = "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;
            string? json = JsonSerializer.Serialize(problem);

            return context.Response.WriteAsync(json);
        }
    }
}
