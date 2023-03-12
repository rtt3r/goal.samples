using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Customers.Aggregates
{
    public class Customer : Entity<string>
    {
        public Customer(string name, string email, DateTime birthdate)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Email = email;
            Birthdate = birthdate;
        }

        // Empty constructor for EF
        protected Customer() { }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime Birthdate { get; private set; }

        public void UpdateName(string name) => Name = name;

        public void UpdateBirthdate(DateTime birthdate) => Birthdate = birthdate;
    }
}
