using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Operation : Entity<string>
{
    protected Operation()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Operation(string name)
        : this()
    {
        SetName(name);
    }

    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string Description { get; protected set; }
    public string ApplicationId { get; protected set; }
    public Application Application { get; protected set; }
    public IEnumerable<Permission> Permissions { get; set; } = new List<Permission>();

    public void SetName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        NormalizedName = Name?.ToUpper();
    }

    public void Describe(string description)
    {
        Description = description?.Trim() ?? throw new ArgumentNullException(nameof(description));
    }
}