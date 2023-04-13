using Microsoft.AspNetCore.Identity;

namespace Goal.Samples.Identity.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }

    public User()
        : base()
    {
    }

    public IEnumerable<Role> MemberOf { get; set; } = new List<Role>();
    public IEnumerable<Authorization> Authorizations { get; set; } = new List<Authorization>();
}