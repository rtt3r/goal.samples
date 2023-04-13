using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class User : Entity<string>
{
    protected User()
    {
        Id = Guid.NewGuid().ToString();
    }

    public User(string userName, string name)
        : this()
    {
        SetName(name);
        UpdateUserName(userName);
    }

    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string UserName { get; protected set; }
    public string NormalizedUserName { get; protected set; }
    public string Email { get; protected set; }
    public string NormalizedEmail { get; protected set; }
    public string ConcurrencyStamp { get; protected set; }
    public IEnumerable<Role> MemberOf { get; protected set; } = new List<Role>();
    public IEnumerable<Authorization> Authorizations { get; protected set; } = new List<Authorization>();

    public void SetName(string name)
    {
        Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        NormalizedName = Name?.Trim()?.ToUpper();
    }

    private void UpdateUserName(string userName)
    {
        UserName = userName?.Trim() ?? throw new ArgumentNullException(nameof(userName));
        NormalizedUserName = UserName?.Trim()?.ToUpper();
    }
}