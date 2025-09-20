using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class SetLogRepository(AppDbContext context) : GenericRepository<SetLog>(context), ISetLogRepository
    {
        private readonly AppDbContext _context = context;
    }
}
