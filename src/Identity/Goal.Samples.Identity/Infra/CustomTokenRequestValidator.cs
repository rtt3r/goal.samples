using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Infra
{
    public sealed class CustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        private readonly UserManager<User> userManager;

        public CustomTokenRequestValidator(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            context.Result.CustomResponse = new Dictionary<string, object>
            {
                { "created_at", DateTimeOffset.UtcNow}
            };

            return Task.CompletedTask;
        }
    }
}
