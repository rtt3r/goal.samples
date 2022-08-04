using Goal.Seedwork.Domain.Events;
using MediatR;

namespace Goal.Demo2.Domain.Customers.Events
{
    public class CustomerRegisteredEvent : Event, INotification
    {
        public CustomerRegisteredEvent(string aggregateId, string name, string email, DateTime birthdate)
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
