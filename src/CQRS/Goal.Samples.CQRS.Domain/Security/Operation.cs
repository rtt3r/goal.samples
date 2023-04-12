using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Operation : Entity<string>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public ICollection<Permission> Permissions { get; private set; }

    protected Operation()
    {
        Id = Guid.NewGuid().ToString();
        Permissions = new HashSet<Permission>();
    }
}
