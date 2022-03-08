using Goal.Demo2.Infra.Data.Configurations;
using Goal.Domain.Seedwork.Events;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo2.Infra.Data.EventSourcing
{
    public class EventSourcingContext : DbContext
    {
        public DbSet<StoredEvent> StoredEvents { get; set; }

        public EventSourcingContext(DbContextOptions<EventSourcingContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StoredEventConfiguration());
        }
    }
}
