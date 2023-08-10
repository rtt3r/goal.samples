using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.SqlServer;

public class SqlServerCoreDbContext : CoreDbContext
{
    public SqlServerCoreDbContext(DbContextOptions options)
        : base(options)
    { }
}
