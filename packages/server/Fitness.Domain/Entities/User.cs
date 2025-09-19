using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        public ICollection<WorkoutTemplate> WorkoutTemplates { get; set; } = [];
        public ICollection<WorkoutSession> WorkoutSessions { get; set; } = [];
    }
}
