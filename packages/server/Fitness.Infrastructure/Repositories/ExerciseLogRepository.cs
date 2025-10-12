using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class ExerciseLogRepository(AppDbContext context) : GenericRepository<ExerciseLog>(context), IExerciseLogRepository
    {
        public readonly AppDbContext _context = context;

        public async Task<int> CountByWorkoutSessionIdAsync(Guid workoutSessionId)
        {
            return await Task.FromResult(_context.ExerciseLogs.Count(el => el.WorkoutSessionId == workoutSessionId));
        }
    }
}
