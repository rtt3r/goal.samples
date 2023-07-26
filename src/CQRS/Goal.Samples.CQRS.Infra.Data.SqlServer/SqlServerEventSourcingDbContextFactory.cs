using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.SqlServer;

public class SqlServerEventSourcingDbContextFactory : DesignTimeDbContextFactory<SqlServerEventSourcingDbContext>
{
    protected override SqlServerEventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<SqlServerEventSourcingDbContext> optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        return new SqlServerEventSourcingDbContext(optionsBuilder.Options);
    }
}
