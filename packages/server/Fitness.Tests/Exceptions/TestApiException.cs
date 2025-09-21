using Fitness.Application.Exceptions;

namespace Fitness.Tests.Exceptions
{
    public class TestApiException(int statusCode, string message) : ApiException(statusCode, message)
    {
    }
}
