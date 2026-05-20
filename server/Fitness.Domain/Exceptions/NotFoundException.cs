namespace Fitness.Domain.Exceptions
{
	public class NotFoundException(Guid entityId, string entityName) : ApiException(404, $"{entityName}: {entityId} is not found.")
	{
	}
}
