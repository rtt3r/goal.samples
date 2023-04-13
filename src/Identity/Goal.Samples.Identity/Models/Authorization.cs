namespace Goal.Samples.Identity.Models;

public class Authorization
{
    protected Authorization()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Authorization(User member, Permission permission)
        : this()
    {
        User = member ?? throw new ArgumentNullException(nameof(member));
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));

        UserId = member.Id;
        PermissionId = permission.Id;
    }

    public Authorization(Role member, Permission permission)
        : this()
    {
        Role = member ?? throw new ArgumentNullException(nameof(member));
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));

        RoleId = member.Id;
        PermissionId = permission.Id;
    }

    public string Id { get; set; }
    public string PermissionId { get; set; }
    public string UserId { get; set; }
    public string RoleId { get; set; }
    public bool Allowed { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
    public Permission Permission { get; set; }

    public void Allow()
        => Allowed = true;

    public void Deny()
        => Allowed = false;
}