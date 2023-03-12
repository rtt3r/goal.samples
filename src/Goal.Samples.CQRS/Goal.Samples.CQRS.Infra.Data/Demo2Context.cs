using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Samples.CQRS.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data
{
    public class Demo2Context : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public Demo2Context(DbContextOptions<Demo2Context> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
