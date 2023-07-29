using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.CQRS.Application.Events.Customers;

public class CustomerRemovedEvent : Event
{
    public CustomerRemovedEvent(string aggregateId)
    {
        AggregateId = aggregateId;
    }
}
