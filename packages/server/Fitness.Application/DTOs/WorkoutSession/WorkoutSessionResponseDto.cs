using Fitness.Application.DTOs.ExerciseLog;
using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.WorkoutSession
{
    public class WorkoutSessionResponseDto
    {
        public Guid Id { get; set; }
        public int Duration { get; set; } = 0;
        public int AverageRestTime { get; set; } = 0;
        public Status Status { get; set; }
        public List<ExerciseLogLightDto> ExerciseLogs { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
