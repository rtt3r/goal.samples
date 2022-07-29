using Goal.Demo2.Domain.Customers.Events;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MediatR;

namespace Goal.Demo2.Application.Bus.Customers
{
    internal class CustomerBusConsumer : EventBusConsumer,
        IConsumer<CustomerRegisteredEvent>,
        IConsumer<CustomerRemovedEvent>,
        IConsumer<CustomerUpdatedEvent>
    {
        public CustomerBusConsumer(
            IMediator mediator,
            IEventStore eventStore)
            : base(mediator, eventStore)
        {
        }

        public async Task Consume(ConsumeContext<CustomerRegisteredEvent> context)
            => await ConsumeEvent(context.Message);

        public async Task Consume(ConsumeContext<CustomerRemovedEvent> context)
            => await ConsumeEvent(context.Message);

        public async Task Consume(ConsumeContext<CustomerUpdatedEvent> context)
            => await ConsumeEvent(context.Message);
    }
}
