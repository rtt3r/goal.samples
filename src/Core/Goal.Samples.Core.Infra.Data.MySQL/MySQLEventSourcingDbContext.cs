using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.MySQL;

public class MySQLEventSourcingDbContext : EventSourcingDbContext
{
    public MySQLEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
