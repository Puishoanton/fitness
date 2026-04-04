using Fitness.Application.DTOs.WorkoutTemplateExercise;

namespace Fitness.Application.DTOs.WorkoutTemplate
{
    public class WorkoutTemplateWithExercisesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<WorkoutTemplateExerciseResponseDto> WorkoutTemplateExercises { get; set; } = [];

    }
}
