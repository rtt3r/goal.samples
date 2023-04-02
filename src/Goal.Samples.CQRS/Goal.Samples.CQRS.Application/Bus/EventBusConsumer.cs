using Goal.Seedwork.Domain.Events;
using MediatR;

namespace Goal.Samples.CQRS.Application.Bus
{
    public abstract class EventBusConsumer
    {
        private readonly IMediator mediator;
        private readonly IEventStore eventStore;

        protected EventBusConsumer(
            IMediator mediator,
            IEventStore eventStore)
        {
            this.mediator = mediator;
            this.eventStore = eventStore;
        }

        protected virtual async Task ConsumeEvent<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            eventStore.Save(@event);
            await mediator.Publish(@event);
        }
    }
}
