using System.ComponentModel.DataAnnotations;

namespace Fitness.Application.DTOs.SetLog
{
    public class CreateSetLogDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Reps must be greater than 0")]
        public int Reps { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Weight must be >= 0")]
        public int Weight { get; set; }

    }
}
