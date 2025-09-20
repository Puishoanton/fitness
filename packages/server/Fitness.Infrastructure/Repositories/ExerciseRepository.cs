using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class ExerciseRepository(AppDbContext context) : GenericRepository<Exercise>(context), IExerciseRepository
    {
        private readonly AppDbContext _context = context;
    }
}
