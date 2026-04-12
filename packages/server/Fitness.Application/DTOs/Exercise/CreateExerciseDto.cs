using System.ComponentModel.DataAnnotations;
using Fitness.Domain.Enums;

namespace Fitness.Application.DTOs.Exercise
{
    public class CreateExerciseDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "MuscleGroup is required")]
        public MuscleGroup MuscleGroup { get; set; }
    }
}
