using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.Core.Application.Events.Customers;

public class CustomerRemovedEvent : Event
{
    public CustomerRemovedEvent(string aggregateId)
    {
        AggregateId = aggregateId;
    }
}
