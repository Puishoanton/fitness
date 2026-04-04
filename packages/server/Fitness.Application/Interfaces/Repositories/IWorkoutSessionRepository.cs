using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IWorkoutSessionRepository : IRepository<WorkoutSession>
    {
        public Task<WorkoutSession?> GetWorkoutSessionByIdWithExerciseLogs(Guid workoutSessionId);
    }
}
