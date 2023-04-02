using System.Diagnostics;
using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MassTransit.Metadata;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Goal.Samples.CQRS.Application.Bus
{
    public abstract class EventConsumer
    {
        private readonly IMediator mediator;
        private readonly IEventStore eventStore;
        private readonly ILogger logger;

        protected EventConsumer(
            IMediator mediator,
            IEventStore eventStore,
            ILogger logger)
        {
            this.mediator = mediator;
            this.eventStore = eventStore;
            this.logger = logger;
        }

        protected virtual async Task ConsumeEvent<TEvent>(ConsumeContext<TEvent> context)
            where TEvent : class, IEvent
        {
            var timer = Stopwatch.StartNew();
            string consumerType = TypeMetadataCache<CustomerRegisteredEvent>.ShortName;

            logger.LogInformation($"Receive event: {consumerType}");

            try
            {
                eventStore.Save(context.Message);
                await mediator.Publish(context.Message);

                await context.NotifyConsumed(timer.Elapsed, consumerType);
            }
            catch (Exception ex)
            {
                await context.NotifyFaulted(timer.Elapsed, consumerType, ex);
            }
        }
    }
}
