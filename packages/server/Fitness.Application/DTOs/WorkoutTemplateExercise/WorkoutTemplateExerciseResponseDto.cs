using Fitness.Application.DTOs.Exercise;

namespace Fitness.Application.DTOs.WorkoutTemplateExercise
{
    public class WorkoutTemplateExerciseResponseDto
    {
        public Guid Id { get; set; }
        public ExerciseLightDto? Exercise { get; set; }
        public int Order { get; set; }
        public int? TargetSets { get; set; }
        public int? TargetReps { get; set; }
        public double? TargetWeight { get; set; }
    }
}
