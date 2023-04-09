using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Infra
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;

        public CustomProfileService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            User user = await _userManager.GetUserAsync(context.Subject);
            ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            context.IssuedClaims.AddRange(principal.Claims.ToList());
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            User user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
