using Microsoft.AspNetCore.Identity;

namespace Goal.Samples.Identity.Models;

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

    public Role(string roleName, Application application)
        : this(roleName)
    {
        SetApplication(application);
    }

    public string Description { get; set; }
    public string ApplicationId { get; set; }
    public Application Application { get; set; }
    public ICollection<User> UserMembers { get; set; } = new List<User>();
    public ICollection<Role> RoleMembers { get; set; } = new List<Role>();
    public ICollection<Role> MemberOf { get; set; } = new List<Role>();
    public ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();

    public void SetApplication(Application application)
    {
        Application = application ?? throw new ArgumentNullException(nameof(application));
        ApplicationId = application.Id;
    }
}