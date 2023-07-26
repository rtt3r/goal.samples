using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.SqlServer;

public class SqlServerCqrsDbContext : CqrsDbContext
{
    public SqlServerCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
