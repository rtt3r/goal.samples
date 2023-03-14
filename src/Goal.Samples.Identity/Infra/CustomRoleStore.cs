using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ritter.Starter.Identity.Data;
using Ritter.Starter.Identity.Models;

namespace Ritter.Starter.Identity.Infra
{
    public class CustomRoleStore : RoleStore<Role, ApplicationDbContext>
    {
        public CustomRoleStore(
            ApplicationDbContext context,
            IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}
