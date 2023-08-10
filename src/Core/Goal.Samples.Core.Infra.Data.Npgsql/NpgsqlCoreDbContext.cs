using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.Npgsql;

public class NpgsqlCoreDbContext : CoreDbContext
{
    public NpgsqlCoreDbContext(DbContextOptions options)
        : base(options)
    { }
}
