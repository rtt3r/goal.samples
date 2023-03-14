using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.CQRS.Api.Application.Events.Customers
{
    public class CustomerRemovedEvent : Event
    {
        public CustomerRemovedEvent(string aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
