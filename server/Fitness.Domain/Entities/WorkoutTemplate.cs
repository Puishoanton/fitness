using System.ComponentModel.DataAnnotations;
using Fitness.Domain.Common;

namespace Fitness.Domain.Entities
{
	public class WorkoutTemplate : BaseEntity
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		public Guid UserId { get; set; }
		public User? User { get; set; }
		public ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = [];
		public ICollection<WorkoutSession> WorkoutSessions { get; set; } = [];
	}
}
