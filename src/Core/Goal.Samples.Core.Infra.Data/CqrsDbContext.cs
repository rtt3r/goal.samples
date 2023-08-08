using Goal.Samples.Core.Domain.Customers.Aggregates;
using Goal.Samples.Core.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data;

public abstract class CqrsDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CqrsDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
