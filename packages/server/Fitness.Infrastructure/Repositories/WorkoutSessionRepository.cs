using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class WorkoutSessionRepository(AppDbContext context) : GenericRepository<WorkoutSession>(context), IWorkoutSessionRepository
    {
        private readonly AppDbContext _context = context;
    }
}
