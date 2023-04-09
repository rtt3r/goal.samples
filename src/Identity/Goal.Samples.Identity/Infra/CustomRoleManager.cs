using Microsoft.AspNetCore.Identity;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Infra
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
