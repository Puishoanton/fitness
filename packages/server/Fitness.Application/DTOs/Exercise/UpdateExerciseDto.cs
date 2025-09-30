using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.Exercise
{
    public class UpdateExerciseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public MuscleGroup? MuscleGroup { get; set; }
    }
}
