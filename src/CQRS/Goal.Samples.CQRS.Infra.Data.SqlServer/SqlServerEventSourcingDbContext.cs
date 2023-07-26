using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.SqlServer;

public class SqlServerEventSourcingDbContext : EventSourcingDbContext
{
    public SqlServerEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
