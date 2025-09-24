using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefreshTokenAsync(string? refreshToken);
    }
}
