using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string MuscleGroup { get; set; } = string.Empty;
        public ICollection<ExerciseLog> ExerciseLogs { get; set; } = [];
        public ICollection<WorkoutTemplate> WorkoutTemplates { get; set; } = [];
    }
}
