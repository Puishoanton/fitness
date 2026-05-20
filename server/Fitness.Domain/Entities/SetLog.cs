using Fitness.Domain.Common;

namespace Fitness.Domain.Entities
{
	public class SetLog : BaseEntity
	{
		public Guid ExerciseLogId { get; set; }
		public ExerciseLog? ExerciseLog { get; set; }
		public int Order { get; set; }
		public int Reps { get; set; }
		public decimal Weight { get; set; }
	}
}
