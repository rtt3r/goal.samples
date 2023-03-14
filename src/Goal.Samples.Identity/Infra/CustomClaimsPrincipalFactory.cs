using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ritter.Starter.Identity.Models;
using Ritter.Starter.Infra.Crosscutting.Extensions;

namespace Ritter.Starter.Identity.Infra
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAcessor)
            : base(userManager, roleManager, optionsAcessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);

            principal.Identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.Name ?? user.UserName));
            principal.Identity.AddClaim(new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")));

            IList<string> roles = await UserManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                principal.Identity.AddClaim(new Claim(JwtClaimTypes.Role, role));
            }

            return principal;
        }
    }
}
