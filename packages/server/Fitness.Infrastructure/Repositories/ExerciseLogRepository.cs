using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class ExerciseLogRepository(AppDbContext context) : GenericRepository<ExerciseLog>(context), IExerciseLogRepository
    {
        public readonly AppDbContext _context = context;
    }
}
