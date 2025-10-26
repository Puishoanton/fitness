using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IExerciseLogRepository : IRepository<ExerciseLog>
    {
        Task<int> CountByWorkoutSessionIdAsync(Guid workoutSessionId);
    }
}
