using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Authorization : Entity<string>
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

    public string PermissionId { get; protected set; }
    public string UserId { get; protected set; }
    public string RoleId { get; protected set; }
    public bool Allowed { get; protected set; }
    public User User { get; protected set; }
    public Role Role { get; protected set; }
    public Permission Permission { get; protected set; }

    public void Allow()
        => Allowed = true;

    public void Deny()
        => Allowed = false;
}