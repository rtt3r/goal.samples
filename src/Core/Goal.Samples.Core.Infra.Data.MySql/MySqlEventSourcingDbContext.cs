using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.MySql;

public class MySqlEventSourcingDbContext : EventSourcingDbContext
{
    public MySqlEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
