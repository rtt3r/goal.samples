namespace Goal.Samples.Identity.Models;

public class Resource
{
    protected Resource()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Resource(string name)
        : this()
    {
        SetName(name);
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string Description { get; set; }
    public string ApplicationId { get; set; }
    public Application Application { get; set; }
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
