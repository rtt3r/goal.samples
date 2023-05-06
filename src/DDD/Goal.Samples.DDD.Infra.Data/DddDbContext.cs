using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Samples.DDD.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.DDD.Infra.Data;

public class DddDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Document> Documents { get; set; }

    public DddDbContext(DbContextOptions<DddDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
    }
}
