using System.Text;
using System.Text.Json.Serialization;
using Fitness.Api.Swagger;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Application.Interfaces.Services;
using Fitness.Application.Interfaces.Validators;
using Fitness.Application.Mapping;
using Fitness.Application.Services;
using Fitness.Application.Validators;
using Fitness.Infrastructure.Data;
using Fitness.Infrastructure.Middlewares;
using Fitness.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    string? clientUrl = builder.Configuration["ClientUrl"];
    if (string.IsNullOrEmpty(clientUrl))
    {
        throw new InvalidOperationException("ClientUrl is missing.");
    }
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(clientUrl)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Fitness API",
            Version = "v1"
        });

        c.OperationFilter<AuthorizeCheckOperationFilter>();
    });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        string? tokenSecretKey = builder.Configuration["Jwt:AccessTokenSecretKey"];
        if (string.IsNullOrEmpty(tokenSecretKey))
        {
            throw new InvalidOperationException("JWT access token secret key is missing.");
        }
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(tokenSecretKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("accessToken"))
                {
                    context.Token = context.Request.Cookies["accessToken"];
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddOpenApi();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();

builder.Services.AddScoped<IWorkoutTemplateRepository, WorkoutTemplateRepository>();
builder.Services.AddScoped<IWorkoutTemplateService, WorkoutTemplateService>();

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
builder.Services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();

builder.Services.AddScoped<IExerciseLogRepository, ExerciseLogRepository>();
builder.Services.AddScoped<IExerciseLogService, ExerciseLogService>();

builder.Services.AddScoped<ISetLogRepository, SetLogRepository>();
builder.Services.AddScoped<ISetLogService, SetLogService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowedOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
