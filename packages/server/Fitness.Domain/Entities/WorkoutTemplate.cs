using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class WorkoutTemplate : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = [];
        public ICollection<WorkoutSession> WorkoutSessions { get; set; } = [];
    }
}
