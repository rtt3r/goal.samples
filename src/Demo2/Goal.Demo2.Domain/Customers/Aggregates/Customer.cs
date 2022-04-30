using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Demo2.Domain.Aggregates.Customers
{
    public class Customer : Entity<string>
    {
        public Customer(string name, string email, DateTime birthDate)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }

        // Empty constructor for EF
        protected Customer() { }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public void UpdateName(string name) => Name = name;

        public void UpdateBirthDate(DateTime birthDate) => BirthDate = birthDate;
    }
}
