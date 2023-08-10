using Goal.Samples.Core.Domain.Customers.Aggregates;
using Goal.Samples.Core.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data;

public abstract class CoreDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CoreDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
