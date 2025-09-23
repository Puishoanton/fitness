using Google.Apis.Auth;

namespace Fitness.Application.Interfaces.Validators
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleJsonWebSignature.Payload> ValidateAsync(string googleTokenId);
    }
}
