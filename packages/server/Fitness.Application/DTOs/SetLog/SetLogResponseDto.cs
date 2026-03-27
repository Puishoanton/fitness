namespace Fitness.Application.DTOs.SetLog
{
    public class SetLogResponseDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
