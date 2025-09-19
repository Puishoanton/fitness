using System.ComponentModel.DataAnnotations;

namespace Fitness.Domain.Entities
{
    public class SetLog : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ExerciseLogId { get; set; }
        public ExerciseLog? ExerciseLog { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
        public int RestTime { get; set; }
    }
}
