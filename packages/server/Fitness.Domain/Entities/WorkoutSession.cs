using System.ComponentModel.DataAnnotations;
using Fitness.Domain.Enums;

namespace Fitness.Domain.Entities
{
    public class WorkoutSession : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid WorkoutTemplateId { get; set; }
        public WorkoutTemplate? WorkoutTemplate { get; set; }
        public int Duration { get; set; } = 0;
        public int AverageRestTime { get; set; } = 0;
        [Required]
        public Status Status { get; set; }
        public ICollection<ExerciseLog> ExerciseLogs { get; set; } = [];
    }
}
