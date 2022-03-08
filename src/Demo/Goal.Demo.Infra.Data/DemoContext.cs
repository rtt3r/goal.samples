using Goal.Demo.Domain.Aggregates.People;
using Goal.Demo.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo.Infra.Data
{
    public class DemoContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        }
    }
}
