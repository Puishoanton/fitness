using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class ExerciseLog : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WorkoutSessionId { get; set; }
        public WorkoutSession? WorkoutSession { get; set; }
        public Guid ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
        public int Order { get; set; }
        public ICollection<SetLog> SetLogs { get; set; } = [];
    }
}
