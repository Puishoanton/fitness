using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.Exercise
{
    public class ExerciseResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public MuscleGroup MuscleGroup { get; set; }
    }
}
