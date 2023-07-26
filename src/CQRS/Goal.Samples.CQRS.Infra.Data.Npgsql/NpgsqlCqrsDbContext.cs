using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.Npgsql;

public class NpgsqlCqrsDbContext : CqrsDbContext
{
    public NpgsqlCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
