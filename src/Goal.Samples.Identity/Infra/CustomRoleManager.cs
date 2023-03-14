using Microsoft.AspNetCore.Identity;
using Ritter.Starter.Identity.Models;

namespace Ritter.Starter.Identity.Infra
{
    public class CustomRoleManager : RoleManager<Role>
    {
        public CustomRoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger)
            : base(
                store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
        }
    }
}
