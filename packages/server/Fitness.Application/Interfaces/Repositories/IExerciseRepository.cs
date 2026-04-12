using Fitness.Domain.Entities;
using Fitness.Domain.Enums;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        public Task<List<MuscleGroup>> GetUniqueMuscleGroupsAsync();
    }
}
