namespace Goal.Samples.Identity.Models;

public class Application
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

    public string Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string Description { get; set; }
    public IEnumerable<Resource> Resources { get; set; } = new List<Resource>();
    public IEnumerable<Operation> Operations { get; set; } = new List<Operation>();
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();

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
