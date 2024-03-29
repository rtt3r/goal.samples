using Goal.Seedwork.Domain.Events;
using MediatR;

namespace Goal.Samples.CQRS.Application.Events.Customers
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

        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public DateTime Birthdate { get; protected set; }
    }
}
