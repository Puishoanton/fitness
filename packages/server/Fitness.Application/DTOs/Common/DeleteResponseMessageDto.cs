namespace Fitness.Application.DTOs.Common
{
    public class DeleteResponseMessageDto (Guid entityId, string entityName) 
    {
        public string Message { get; } = $"{entityName} with id {entityId} has been deleted successfully.";
    }
}
