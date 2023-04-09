using Microsoft.AspNetCore.Identity;

namespace Goal.Samples.Identity.Models
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