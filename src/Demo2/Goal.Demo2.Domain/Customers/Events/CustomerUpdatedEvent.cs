using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Domain.Customers.Events
{
    public class CustomerUpdatedEvent : Event
    {
        public CustomerUpdatedEvent(string aggregateId, string name, string email, DateTime birthdate)
        {
            AggregateId = aggregateId;
            Name = name;
            Email = email;
            Birthdate = birthdate;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthdate { get; private set; }
    }
}
