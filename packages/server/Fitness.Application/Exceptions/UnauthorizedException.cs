using Microsoft.AspNetCore.Http;

namespace Fitness.Application.Exceptions
{
    public class UnauthorizedException() : ApiException(StatusCodes.Status401Unauthorized, "Refresh token is required.")
    {
    }
}
