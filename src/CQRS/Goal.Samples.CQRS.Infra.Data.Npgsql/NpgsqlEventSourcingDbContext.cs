using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.Npgsql;

public class NpgsqlEventSourcingDbContext : EventSourcingDbContext
{
    public NpgsqlEventSourcingDbContext(DbContextOptions options)
        : base(options)
    { }
}
