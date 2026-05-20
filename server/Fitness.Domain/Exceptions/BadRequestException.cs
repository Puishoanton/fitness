namespace Fitness.Domain.Exceptions
{
	public class BadRequestException(string message) : ApiException(400, message)
	{
	}
}
