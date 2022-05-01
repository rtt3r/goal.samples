using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Domain.Customers.Events
{
    public class CustomerRemovedEvent : Event
    {
        public CustomerRemovedEvent(string aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
