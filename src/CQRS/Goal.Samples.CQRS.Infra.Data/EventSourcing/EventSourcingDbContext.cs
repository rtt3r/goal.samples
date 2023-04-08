using Goal.Samples.CQRS.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing
{
    public class EventSourcingDbContext : DbContext
    {
        public DbSet<StoredEvent> StoredEvents { get; set; }

        public EventSourcingDbContext(DbContextOptions<EventSourcingDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new StoredEventConfiguration());
        }
    }
}
