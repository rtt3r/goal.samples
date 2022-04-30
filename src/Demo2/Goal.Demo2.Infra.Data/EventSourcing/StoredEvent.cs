using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Infra.Data.EventSourcing
{
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

        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Data { get; private set; }
        public string User { get; private set; }
    }
}
