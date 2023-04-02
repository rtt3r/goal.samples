using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Goal.Samples.CQRS.Application.Bus.Consumers
{
    public class CustomerRegisteredEventConsumer : EventConsumer, IConsumer<CustomerRegisteredEvent>
    {
        public CustomerRegisteredEventConsumer(
            IMediator mediator,
            IEventStore eventStore,
            ILogger<CustomerRegisteredEventConsumer> logger)
            : base(mediator, eventStore, logger)
        {
        }

        public async Task Consume(ConsumeContext<CustomerRegisteredEvent> context)
            => await ConsumeEvent(context);

        public class ConsumerDefinition : ConsumerDefinition<CustomerRegisteredEventConsumer>
        {
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CustomerRegisteredEventConsumer> consumerConfigurator) => consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        }
    }
}
