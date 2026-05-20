using System.ComponentModel.DataAnnotations;
using Fitness.Domain.Common;

namespace Fitness.Domain.Entities
{
	public class User : BaseEntity
	{
		[Required]
		public string Email { get; set; } = string.Empty;
		public string? RefreshToken { get; set; }
		public ICollection<WorkoutTemplate> WorkoutTemplates { get; set; } = [];
		public ICollection<WorkoutSession> WorkoutSessions { get; set; } = [];
	}
}
