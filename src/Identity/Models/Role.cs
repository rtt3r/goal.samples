using Microsoft.AspNetCore.Identity;

namespace Ritter.Starter.Identity.Models
{
    public class Role : IdentityRole
    {
        public Role()
            : base()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }
    }
}