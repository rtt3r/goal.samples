using System.Text.Json;
using Goal.Samples.Infra.Crosscutting;
using Goal.Seedwork.Domain.Events;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly EventSourcingContext dbContext;
        private readonly AppState appState;

        public SqlEventStore(
            EventSourcingContext dbContext,
            AppState appState)
        {
            this.dbContext = dbContext;
            this.appState = appState;
        }

        public void Save<T>(T @event) where T : IEvent
        {
            var storedEvent = new StoredEvent(
               @event,
               JsonSerializer.Serialize(@event),
               appState.User.UserId);

            dbContext.StoredEvents.Add(storedEvent);
            dbContext.SaveChanges();
        }
    }
}
