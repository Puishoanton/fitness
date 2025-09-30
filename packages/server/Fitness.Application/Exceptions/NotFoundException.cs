using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Exceptions
{
    public class NotFoundException(Guid entityId, string entityName) : ApiException(StatusCodes.Status404NotFound, $"{entityName}: {entityId} is not found.")
    {
    }
}
