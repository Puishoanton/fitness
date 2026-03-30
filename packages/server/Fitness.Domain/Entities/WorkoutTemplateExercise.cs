using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class WorkoutTemplateExercise : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid WorkoutTemplateId { get; set; }
        public WorkoutTemplate? WorkoutTemplate { get; set; }

        public Guid ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        public int Order { get; set; }

        public int? TargetSets { get; set; }
        public int? TargetReps { get; set; }
        public double? TargetWeight { get; set; }

    }
}
