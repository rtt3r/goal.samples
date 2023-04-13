using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Samples.CQRS.Domain.Security;
using Goal.Samples.CQRS.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
