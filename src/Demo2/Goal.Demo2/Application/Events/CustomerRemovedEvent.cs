using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Application.Events
{
    public class CustomerRemovedEvent : Event
    {
        public CustomerRemovedEvent(Guid id)
        {
            AggregateId = $"{id}";
        }
    }
}
