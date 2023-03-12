using Microsoft.AspNetCore.Identity;

namespace Ritter.Starter.Identity.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public User()
            : base()
        {
        }
    }
}