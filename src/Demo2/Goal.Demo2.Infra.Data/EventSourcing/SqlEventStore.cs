using System.Text.Json;
using Goal.Seedwork.Domain.Events;

namespace Goal.Demo2.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly EventSourcingContext dbContext;

        public SqlEventStore(EventSourcingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save<T>(T @event) where T : IEvent
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
