using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.Core.Infra.Data.SqlServer;

public class SqlServerCqrsDbContextFactory : DesignTimeDbContextFactory<SqlServerCqrsDbContext>
{
    protected override SqlServerCqrsDbContext CreateNewInstance(DbContextOptionsBuilder<SqlServerCqrsDbContext> optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        return new SqlServerCqrsDbContext(optionsBuilder.Options);
    }
}
