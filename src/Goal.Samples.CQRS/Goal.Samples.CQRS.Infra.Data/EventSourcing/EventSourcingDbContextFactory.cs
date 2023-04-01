using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing
{
    public class EventSourcingDbContextFactory : DesignTimeDbContextFactory<EventSourcingDbContext>
    {
        protected override EventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<EventSourcingDbContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new EventSourcingDbContext(optionsBuilder.Options);
        }
    }
}
