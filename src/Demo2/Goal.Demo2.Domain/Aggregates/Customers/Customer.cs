using Goal.Domain.Seedwork.Aggregates;

namespace Goal.Demo2.Domain.Aggregates.Customers
{
    public class Customer : Entity
    {
        public Customer(string name, string email, DateTime birthDate)
        {
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }

        // Empty constructor for EF
        protected Customer() { }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public void UpdateName(string name) => throw new NotImplementedException();
        public void UpdateBirthDate(DateTime birthDate) => throw new NotImplementedException();
    }
}
