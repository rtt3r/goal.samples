using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing;

public class StoredEvent : Event
{
    public StoredEvent(IEvent @event, string data, string user)
    {
        AggregateId = @event.AggregateId;
        EventType = @event.EventType;
        Data = data;
        User = user;
    }

    protected StoredEvent() { }

    public string Id { get; protected set; } = Guid.NewGuid().ToString();
    public string Data { get; protected set; }
    public string User { get; protected set; }
}
