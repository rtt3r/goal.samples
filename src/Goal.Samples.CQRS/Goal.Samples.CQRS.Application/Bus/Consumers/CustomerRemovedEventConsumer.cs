using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Goal.Samples.CQRS.Application.Bus.Consumers
{
    public class CustomerRemovedEventConsumer : EventConsumer, IConsumer<CustomerRemovedEvent>
    {
        public CustomerRemovedEventConsumer(
            IMediator mediator,
            IEventStore eventStore,
            ILogger<CustomerRemovedEventConsumer> logger)
            : base(mediator, eventStore, logger)
        {
        }

        public async Task Consume(ConsumeContext<CustomerRemovedEvent> context)
            => await ConsumeEvent(context);

        public class ConsumerDefinition : ConsumerDefinition<CustomerRemovedEventConsumer>
        {
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CustomerRemovedEventConsumer> consumerConfigurator) => consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        }
    }
}
