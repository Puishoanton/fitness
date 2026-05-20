using System.ComponentModel.DataAnnotations;
using Fitness.Domain.Common;
using Fitness.Domain.Enums;

namespace Fitness.Domain.Entities
{
	public class Exercise : BaseEntity
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public MuscleGroup MuscleGroup { get; set; }
		public ICollection<ExerciseLog> ExerciseLogs { get; set; } = [];
		public ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = [];
	}
}
