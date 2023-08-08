using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.Npgsql;

public class NpgsqlCqrsDbContext : CqrsDbContext
{
    public NpgsqlCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
