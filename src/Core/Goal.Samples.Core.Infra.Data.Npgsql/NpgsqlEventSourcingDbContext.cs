using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.Npgsql;

public class NpgsqlEventSourcingDbContext : EventSourcingDbContext
{
    public NpgsqlEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
