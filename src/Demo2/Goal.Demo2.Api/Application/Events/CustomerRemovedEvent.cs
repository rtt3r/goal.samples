using Goal.Domain.Seedwork.Events;

namespace Goal.Demo2.Api.Application.Events
{
    public class CustomerRemovedEvent : Event
    {
        public CustomerRemovedEvent(Guid id)
        {
            AggregateId = id;
        }
    }
}
