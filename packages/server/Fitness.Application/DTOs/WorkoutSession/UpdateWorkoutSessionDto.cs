using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.WorkoutSession
{
    public class UpdateWorkoutSessionDto
    {
        public int? Duration { get; set; }
        public int? AverageRestTime { get; set; }
        public Status? Status { get; set; }
    }
}
