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
            string consumerName = TypeMetadataCache<CustomerRegisteredEvent>.ShortName;

            logger.LogInformation("{InformationData} Receive event.", consumerName);

            try
            {
                await HandleEvent(context.Message);
                eventStore.Save(context.Message);

                logger.LogInformation("{InformationData}: Successfully consumed event.", consumerName);
                await context.NotifyConsumed(timer.Elapsed, consumerName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{InformationData}: An error occurred while consuming an event.", consumerName);
                await context.NotifyFaulted(timer.Elapsed, consumerName, ex);
            }
        }

        protected abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
    }
}
