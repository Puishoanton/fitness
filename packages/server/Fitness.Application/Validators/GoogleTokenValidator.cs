using Fitness.Application.Interfaces.Validators;
using Google.Apis.Auth;

namespace Fitness.Application.Validators
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        public Task<GoogleJsonWebSignature.Payload> ValidateAsync(string googleTokenId) =>
             GoogleJsonWebSignature.ValidateAsync(googleTokenId);
    }
}
