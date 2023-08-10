using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.Core.Infra.Data.MySQL;

public class MySQLCoreDbContextFactory : DesignTimeDbContextFactory<MySQLCoreDbContext>
{
    protected override MySQLCoreDbContext CreateNewInstance(DbContextOptionsBuilder<MySQLCoreDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySQL(Configuration.GetConnectionString("MySQLConnection"));
        return new MySQLCoreDbContext(optionsBuilder.Options);
    }
}
