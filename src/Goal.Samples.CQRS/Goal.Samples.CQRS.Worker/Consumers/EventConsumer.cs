using System.Diagnostics;
using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MassTransit.Metadata;

namespace Goal.Samples.CQRS.Worker.Consumers
{
    public abstract class EventConsumer<TEvent> : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        private readonly IEventStore eventStore;
        private readonly ILogger logger;

        protected EventConsumer(
            IEventStore eventStore,
            ILogger logger)
        {
            this.eventStore = eventStore;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var timer = Stopwatch.StartNew();
            string consumerType = TypeMetadataCache<CustomerRegisteredEvent>.ShortName;

            logger.LogInformation($"Receive event: {consumerType}");

            try
            {
                await HandleEvent(context.Message);

                eventStore.Save(context.Message);
                await context.NotifyConsumed(timer.Elapsed, consumerType);
            }
            catch (Exception ex)
            {
                await context.NotifyFaulted(timer.Elapsed, consumerType, ex);
            }
        }

        protected abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
    }
}
