using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing
{
    public class EventSourcingDesignTimeDbContextFactory : DesignTimeDbContextFactory<EventSourcingContext>
    {
        protected override EventSourcingContext CreateNewInstance(DbContextOptionsBuilder<EventSourcingContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new EventSourcingContext(optionsBuilder.Options);
        }
    }
}
