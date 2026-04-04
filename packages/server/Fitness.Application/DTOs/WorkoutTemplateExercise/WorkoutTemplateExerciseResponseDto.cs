namespace Fitness.Application.DTOs.WorkoutTemplateExercise
{
    public class WorkoutTemplateExerciseResponseDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public int? TargetSets { get; set; }
        public int? TargetReps { get; set; }
        public double? TargetWeight { get; set; }

    }
}
