using System.Linq.Expressions;
using Fitness.Domain.Entities;

namespace Fitness.Application.Interfaces.Repositories
{
    public interface IWorkoutTemplateExerciseRepository : IRepository<WorkoutTemplateExercise>
    {
        public Task<List<WorkoutTemplateExercise>> GetAllByWorkoutTemplateIdAsync(Guid workoutTemplateId);
        public Task<int> CountByWorkoutTemplateIdAsync(Guid workoutTemplateId);
        public Task SaveChangesAsync();
        public Task<bool> ExistsAsync(Expression<Func<WorkoutTemplateExercise, bool>> predicate);
    }
}
