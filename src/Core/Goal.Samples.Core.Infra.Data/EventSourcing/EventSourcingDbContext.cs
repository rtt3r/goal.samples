using Goal.Samples.Core.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.EventSourcing;

public abstract class EventSourcingDbContext : DbContext
{
    public DbSet<StoredEvent> StoredEvents { get; set; }

    public EventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new StoredEventConfiguration());
    }
}
