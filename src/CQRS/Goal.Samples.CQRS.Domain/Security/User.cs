using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class User : Entity<string>
{
    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }
    public string UserName { get; protected set; }
    public string NormalizedUserName { get; protected set; }
    public string Email { get; protected set; }
    public string NormalizedEmail { get; protected set; }
    public string ConcurrencyStamp { get; protected set; }
    public bool Active { get; protected set; }

    protected User()
    {
        Id = Guid.NewGuid().ToString();
        Active = true;
    }

    public User(string userName, string name)
        : this()
    {
        UpdateUserName(userName);
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

    public void UpdateEmail(string email)
    {
        Email = email?.Trim();
        NormalizedEmail = NormalizeData(Email);
    }

    private void UpdateName(string name)
    {
        Name = name?.Trim();
        NormalizedName = NormalizeData(Name);
    }

    private void UpdateUserName(string userName)
    {
        UserName = userName?.Trim();
        NormalizedUserName = NormalizeData(UserName);
    }

    private static string NormalizeData(string data)
        => data?.Trim()?.ToUpper();
}