namespace Fitness.Application.DTOs.User
{
    public class UserPayloadDto
    {
        public Guid Id { get; set; }
        public string Email{ get; set; } = string.Empty;
    }
}
