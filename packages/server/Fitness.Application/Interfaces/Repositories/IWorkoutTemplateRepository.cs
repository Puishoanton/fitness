using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IWorkoutTemplateRepository : IRepository<WorkoutTemplate>
    {
        public Task<WorkoutTemplate?> GetWorkoutTemplateByIdWithExercises (Guid workoutTemplateId);
    }
}
