using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Exceptions
{
    public class NotFoundException(string message) : ApiException(StatusCodes.Status404NotFound, message)
    {
    }
}
