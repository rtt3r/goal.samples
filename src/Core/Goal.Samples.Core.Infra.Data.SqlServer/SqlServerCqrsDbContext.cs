using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.SqlServer;

public class SqlServerCqrsDbContext : CqrsDbContext
{
    public SqlServerCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
