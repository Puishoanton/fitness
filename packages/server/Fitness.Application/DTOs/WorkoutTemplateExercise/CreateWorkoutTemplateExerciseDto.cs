namespace Fitness.Application.DTOs.WorkoutTemplateExercise
{
    public class CreateWorkoutTemplateExerciseDto
    {
        public Guid ExerciseId { get; set; }
        public int? TargetSets { get; set; }
        public int? TargetReps { get; set; }
        public double? TargetWeight { get; set; }
    }
}
