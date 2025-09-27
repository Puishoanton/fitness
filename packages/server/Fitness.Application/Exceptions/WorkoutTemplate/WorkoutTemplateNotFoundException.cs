using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Exceptions.WorkoutTemplate
{
    public class WorkoutTemplateNotFoundException (Guid workoutTemplateId) : ApiException(StatusCodes.Status404NotFound, $"WorkoutTemplateId: {workoutTemplateId} is not found.")
    {
    }
}
