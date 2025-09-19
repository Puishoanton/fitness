using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class WorkoutTemplateRepository(AppDbContext context) : GenericRepository<WorkoutTemplate>(context), IWorkoutTemplateRepository
    {
        private readonly AppDbContext _context = context;
    }
}
