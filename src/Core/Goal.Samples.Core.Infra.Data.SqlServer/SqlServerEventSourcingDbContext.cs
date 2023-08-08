using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.SqlServer;

public class SqlServerEventSourcingDbContext : EventSourcingDbContext
{
    public SqlServerEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
