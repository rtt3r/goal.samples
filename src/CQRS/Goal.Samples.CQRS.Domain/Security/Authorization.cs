using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Authorization : Entity<string>
{
    public string PermissionId { get; private set; }
    public string RoleId { get; private set; }
    public bool Allowed { get; private set; }
    public Role Role { get; private set; }
    public Permission Permission { get; private set; }

    protected Authorization()
    {
        Id = Guid.NewGuid().ToString();
    }
}
