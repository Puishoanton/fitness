namespace Fitness.Domain.Exceptions
{
	public class UnauthorizedException() : ApiException(401, "Refresh token is required.")
	{
	}
}
