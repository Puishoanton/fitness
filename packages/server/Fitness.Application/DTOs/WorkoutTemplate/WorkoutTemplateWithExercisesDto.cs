using Fitness.Application.DTOs.Exercise;

namespace Fitness.Application.DTOs.WorkoutTemplate
{
    public class WorkoutTemplateWithExercisesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseLightDto> Exercises { get; set; } = [];

    }
}
