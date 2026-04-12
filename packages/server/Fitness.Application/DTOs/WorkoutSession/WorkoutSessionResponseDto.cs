using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.WorkoutSession
{
    public class WorkoutSessionResponseDto
    {
        public Guid Id { get; set; }
        public int Duration { get; set; } = 0;
        public Status Status { get; set; }
        public List<ExerciseLogLightDto> ExerciseLogs { get; set; } = [];
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
