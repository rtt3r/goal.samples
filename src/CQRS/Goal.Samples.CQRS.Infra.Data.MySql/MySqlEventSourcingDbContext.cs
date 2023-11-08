using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.MySql;

public class MySqlEventSourcingDbContext : EventSourcingDbContext
{
    public MySqlEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
