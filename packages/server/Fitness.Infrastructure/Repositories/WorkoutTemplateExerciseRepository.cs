using System.Linq.Expressions;
using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Infrastructure.Repositories
{
    public class WorkoutTemplateExerciseRepository(AppDbContext context) : GenericRepository<WorkoutTemplateExercise>(context), IWorkoutTemplateExerciseRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<WorkoutTemplateExercise>> GetAllByWorkoutTemplateIdAsync(Guid workoutTemplateId)
        {
            return await _context.WorkoutTemplateExercises
                .Where(wte => wte.WorkoutTemplateId == workoutTemplateId)
                .OrderBy(wte => wte.Order)
                .ToListAsync();
        }

        public async Task<int> CountByWorkoutTemplateIdAsync(Guid workoutTemplateId)
        {
            return await _context.WorkoutTemplateExercises
                .CountAsync(wte => wte.WorkoutTemplateId == workoutTemplateId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<WorkoutTemplateExercise, bool>> predicate)
        {
            return await _context.WorkoutTemplateExercises.AnyAsync(predicate);
        }
    }
}
