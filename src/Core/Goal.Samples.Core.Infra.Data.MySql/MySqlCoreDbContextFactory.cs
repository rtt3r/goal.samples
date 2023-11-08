using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.Core.Infra.Data.MySql;

public class MySqlCoreDbContextFactory : DesignTimeDbContextFactory<MySqlCoreDbContext>
{
    protected override MySqlCoreDbContext CreateNewInstance(DbContextOptionsBuilder<MySqlCoreDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySql(
            Configuration.GetConnectionString("MySqlConnection"),
            new MySqlServerVersion(new Version(8, 2, 0))
        );

        return new MySqlCoreDbContext(optionsBuilder.Options);
    }
}
