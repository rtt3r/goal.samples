using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.MySql;

public class MySqlCqrsDbContextFactory : DesignTimeDbContextFactory<MySqlCqrsDbContext>
{
    protected override MySqlCqrsDbContext CreateNewInstance(DbContextOptionsBuilder<MySqlCqrsDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySql(
            Configuration.GetConnectionString("MySqlConnection"),
            new MySqlServerVersion(new Version(8, 2, 0))
        );

        return new MySqlCqrsDbContext(optionsBuilder.Options);
    }
}
