using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Application : Entity<string>
{
    protected Application()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Application(string name)
        : this()
    {
        SetName(name);
    }

    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string Description { get; protected set; }
    public IEnumerable<Resource> Resources { get; protected set; } = new List<Resource>();
    public IEnumerable<Operation> Operations { get; protected set; } = new List<Operation>();
    public IEnumerable<Role> Roles { get; protected set; } = new List<Role>();

    public void SetName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        NormalizedName = Name?.ToUpper();
    }

    public void Describe(string description)
    {
        Description = description?.Trim() ?? throw new ArgumentNullException(nameof(description));
    }

    public void AddOperation(Operation operation)
    {
        if (operation is null)
            throw new ArgumentNullException(nameof(operation));

        Operations = Operations
            .Append(operation)
            .ToList();
    }

    public void AddResource(Resource resource)
    {
        if (resource is null)
            throw new ArgumentNullException(nameof(resource));

        Resources = Resources
            .Append(resource)
            .ToList();
    }

    public void AddRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        Roles = Roles
            .Append(role)
            .ToList();
    }
}
