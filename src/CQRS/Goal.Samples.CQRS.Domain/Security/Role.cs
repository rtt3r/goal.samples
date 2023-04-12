using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Role : Entity<string>
{
    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string ConcurrencyStamp { get; protected set; }
    public string Description { get; protected set; }
    public bool Active { get; protected set; } = true;
    public ICollection<Authorization> Authorizations { get; protected set; }

    protected Role()
    {
        Id = Guid.NewGuid().ToString();
        Active = true;
        Authorizations = new HashSet<Authorization>();
    }

    public Role(string name)
        : this()
    {
        UpdateName(name);
    }

    public void Activate()
    {
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
    }

    public void UpdateDescription(string description)
    {
        Description = description?.Trim();
    }

    public void UpdateName(string name)
    {
        Name = name?.Trim();
        NormalizedName = Name?.ToUpper();
    }
}
