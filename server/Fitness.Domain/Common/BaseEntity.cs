using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Common
{
	public abstract class BaseEntity
	{
		[Key]
		public Guid Id { get; set; }
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
		public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
	}
}
