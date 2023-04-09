using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Goal.Samples.Identity.Data;
using Goal.Samples.Identity.Models;

namespace Goal.Samples.Identity.Infra
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
