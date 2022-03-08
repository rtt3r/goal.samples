using Goal.Infra.Data.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo2.Infra.Data.EventSourcing
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
