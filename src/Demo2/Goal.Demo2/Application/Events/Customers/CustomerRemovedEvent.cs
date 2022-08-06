using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Application.Events.Customers
{
    public class CustomerRemovedEvent : Event
    {
        public CustomerRemovedEvent(string aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
