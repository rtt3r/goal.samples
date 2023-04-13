using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Role : Entity<string>
{
    protected Role()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Role(string name)
        : this()
    {
        SetName(name);
    }

    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string ConcurrencyStamp { get; protected set; }
    public string Description { get; protected set; }
    public string ApplicationId { get; protected set; }
    public Application Application { get; protected set; }
    public ICollection<User> UserMembers { get; protected set; } = new List<User>();
    public ICollection<Role> RoleMembers { get; protected set; } = new List<Role>();
    public ICollection<Role> MemberOf { get; protected set; } = new List<Role>();
    public ICollection<Authorization> Authorizations { get; protected set; } = new List<Authorization>();

    public void SetName(string name)
    {
        Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        NormalizedName = Name?.ToUpper();
    }

    public void Describe(string description)
    {
        Description = description?.Trim() ?? throw new ArgumentNullException(nameof(description));
    }
}
