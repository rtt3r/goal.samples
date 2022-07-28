using Goal.Demo2.Domain.Customers.Events;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MediatR;

namespace Goal.Demo2.Application.Bus.Customers
{
    public class CustomerBusConsumer :
        IConsumer<CustomerRegisteredEvent>,
        IConsumer<CustomerRemovedEvent>,
        IConsumer<CustomerUpdatedEvent>
    {
        private readonly IMediator mediator;
        private readonly IEventStore eventStore;

        public CustomerBusConsumer(
            IMediator mediator,
            IEventStore eventStore)
        {
            this.mediator = mediator;
            this.eventStore = eventStore;
        }

        public async Task Consume(ConsumeContext<CustomerRegisteredEvent> context)
        {
            eventStore.Save(context.Message);
            await mediator.Publish(context.Message);
        }

        public async Task Consume(ConsumeContext<CustomerRemovedEvent> context)
        {
            eventStore.Save(context.Message);
            await mediator.Publish(context.Message);
        }

        public async Task Consume(ConsumeContext<CustomerUpdatedEvent> context)
        {
            eventStore.Save(context.Message);
            await mediator.Publish(context.Message);
        }
    }
}
