using System.Text.Json;
using Goal.Domain.Seedwork.Events;

namespace Goal.Demo2.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly EventSourcingContext dbContext;

        public SqlEventStore(EventSourcingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save<T>(T @event) where T : Event
        {
            var storedEvent = new StoredEvent(
                @event,
                JsonSerializer.Serialize(@event),
                "");

            dbContext.StoredEvents.Add(storedEvent);
            dbContext.SaveChanges();
        }
    }
}
