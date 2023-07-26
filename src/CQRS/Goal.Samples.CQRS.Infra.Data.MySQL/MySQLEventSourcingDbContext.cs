using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.MySQL;

public class MySQLEventSourcingDbContext : EventSourcingDbContext
{
    public MySQLEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
