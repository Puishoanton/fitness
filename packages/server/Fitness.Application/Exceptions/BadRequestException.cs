using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Exceptions
{
    public class BadRequestException(string message) : ApiException(StatusCodes.Status400BadRequest, message)
    {
    }
}
