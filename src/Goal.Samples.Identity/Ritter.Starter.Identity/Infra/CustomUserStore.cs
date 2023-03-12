using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Ritter.Starter.Identity.Data;
using Ritter.Starter.Identity.Models;

namespace Ritter.Starter.Identity.Infra
{
    public class CustomUserStore : UserStore<User, Role, ApplicationDbContext>
    {
        public CustomUserStore(
            ApplicationDbContext context,
            IdentityErrorDescriber describer)
            : base(context, describer)
        {
        }
    }
}
