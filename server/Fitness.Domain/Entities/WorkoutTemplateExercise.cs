using Fitness.Domain.Common;

namespace Fitness.Domain.Entities
{
	public class WorkoutTemplateExercise : BaseEntity
	{
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
