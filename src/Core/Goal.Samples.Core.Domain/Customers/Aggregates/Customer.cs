using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.Core.Domain.Customers.Aggregates;

public class Customer : Entity<string>
{
    public Customer(string name, string email, DateTime birthDate)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Email = email;
        Birthdate = birthDate;
    }

    // Empty constructor for EF
    protected Customer() { }

    public string Name { get; protected set; }

    public string Email { get; protected set; }

    public DateTime Birthdate { get; protected set; }

    public void UpdateName(string name) => Name = name;

    public void UpdateBirthdate(DateTime birthDate) => Birthdate = birthDate;
}
