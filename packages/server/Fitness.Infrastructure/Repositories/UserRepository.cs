using Fitness.Application.Interfaces.Repositories;
using Fitness.Domain.Entities;
using Fitness.Infrastructure.Data;

namespace Fitness.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        private readonly AppDbContext _context = context;
    }
}
