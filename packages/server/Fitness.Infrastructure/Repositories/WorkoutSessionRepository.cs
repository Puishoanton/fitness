using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Infrastructure.Repositories
{
    public class WorkoutSessionRepository(AppDbContext context) : GenericRepository<WorkoutSession>(context), IWorkoutSessionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<WorkoutSession?> GetWorkoutSessionByIdWithExerciseLogs(Guid workoutSessionId)
        {
            return await _context.WorkoutSessions
                .Include(wt => wt.ExerciseLogs)
                .FirstOrDefaultAsync(wt => wt.Id == workoutSessionId);
        }
    }
}
