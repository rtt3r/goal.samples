using System.Diagnostics;
using Goal.Seedwork.Domain.Events;
using MassTransit;
using MassTransit.Metadata;

namespace Goal.Samples.Core.Worker.Consumers;

public abstract class EventConsumer<TEvent> : IConsumer<TEvent>
    where TEvent : class, IEvent
{
    private readonly IEventStore eventStore;
    private readonly ILogger logger;

    protected virtual string ConsumerName { get; } = TypeMetadataCache<TEvent>.ShortName;

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

        logger.LogInformation("{InformationData} Received event.", ConsumerName);

        try
        {
            await HandleEvent(context.Message);
            eventStore.Save(context.Message);

            timer.Stop();
            logger.LogInformation("{InformationData}: Successfully consumed event.", ConsumerName);
            await context.NotifyConsumed(timer.Elapsed, ConsumerName);
        }
        catch (Exception ex)
        {
            timer.Stop();
            logger.LogError(ex, "{InformationData}: An error occurred while consuming an event.", ConsumerName);
            await context.NotifyFaulted(timer.Elapsed, ConsumerName, ex);
        }
    }

    protected abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
}
