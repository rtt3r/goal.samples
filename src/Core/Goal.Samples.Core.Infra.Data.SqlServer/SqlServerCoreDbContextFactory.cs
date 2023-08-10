using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.Core.Infra.Data.SqlServer;

public class SqlServerCoreDbContextFactory : DesignTimeDbContextFactory<SqlServerCoreDbContext>
{
    protected override SqlServerCoreDbContext CreateNewInstance(DbContextOptionsBuilder<SqlServerCoreDbContext> optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        return new SqlServerCoreDbContext(optionsBuilder.Options);
    }
}
