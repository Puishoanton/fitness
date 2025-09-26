using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Infrastructure.Repositories
{
    public class WorkoutTemplateRepository(AppDbContext context) : GenericRepository<WorkoutTemplate>(context), IWorkoutTemplateRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<WorkoutTemplate?> GetWorkoutTemplateByIdWithExercises(Guid workoutTemplateId)
        {
            return await _context.WorkoutTemplates
                .Include(wt => wt.Exercises)
                .FirstOrDefaultAsync(wt=> wt.Id == workoutTemplateId);
        }
    }
}
