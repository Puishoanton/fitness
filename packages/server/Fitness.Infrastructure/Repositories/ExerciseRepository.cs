using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Domain.Enums;
using Fitness.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Infrastructure.Repositories
{
    public class ExerciseRepository(AppDbContext context) : GenericRepository<Exercise>(context), IExerciseRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<MuscleGroup>> GetUniqueMuscleGroupsAsync()
        {
            return await _context.Exercises
                .Select(e => e.MuscleGroup)
                .Distinct()
                .ToListAsync();
        }

        public async Task<ICollection<Exercise>> GetAllWithMuscleGroupAsync(MuscleGroup? muscleGroup)
        {
            IQueryable<Exercise> query = _context.Exercises;
            if (muscleGroup.HasValue)
            {
                query = query.Where(e => e.MuscleGroup == muscleGroup);
            }

            return await query.ToListAsync();
        }
    }
}
