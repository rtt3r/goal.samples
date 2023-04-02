using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Goal.Samples.CQRS.Application.Bus.Consumers
{
    public class CustomerUpdatedEventConsumer : EventConsumer, IConsumer<CustomerUpdatedEvent>
    {
        public CustomerUpdatedEventConsumer(
            IMediator mediator,
            IEventStore eventStore,
            ILogger<CustomerUpdatedEventConsumer> logger)
            : base(mediator, eventStore, logger)
        {
        }

        public async Task Consume(ConsumeContext<CustomerUpdatedEvent> context)
            => await ConsumeEvent(context);

        public class ConsumerDefinition : ConsumerDefinition<CustomerUpdatedEventConsumer>
        {
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CustomerUpdatedEventConsumer> consumerConfigurator) => consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        }
    }
}
