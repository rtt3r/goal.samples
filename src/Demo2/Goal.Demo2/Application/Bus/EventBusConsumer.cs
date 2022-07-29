using Goal.Seedwork.Domain.Events;
using MediatR;

namespace Goal.Demo2.Application.Bus
{
    internal abstract class EventBusConsumer
    {
        private readonly IMediator mediator;
        private readonly IEventStore eventStore;

        public EventBusConsumer(
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
