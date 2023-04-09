using Goal.Samples.Identity.Data;
using Goal.Samples.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Goal.Samples.Identity.Infra;

public class CustomRoleStore : RoleStore<Role, ApplicationDbContext>
{
    public CustomRoleStore(
        ApplicationDbContext context,
        IdentityErrorDescriber describer = null)
        : base(context, describer)
    {
    }
}
