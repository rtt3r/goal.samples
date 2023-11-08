using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.Core.Application.Events.Customers;

public class CustomerUpdatedEvent : Event
{
    public CustomerUpdatedEvent(string aggregateId, string name, string email, DateTime birthDate)
    {
        AggregateId = aggregateId;
        Name = name;
        Email = email;
        Birthdate = birthDate;
    }

    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public DateTime Birthdate { get; protected set; }
}
