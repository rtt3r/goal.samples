using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Samples.CQRS.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data;

public class SampleDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
