using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.MySql;

public class MySqlEventSourcingDbContextFactory : DesignTimeDbContextFactory<MySqlEventSourcingDbContext>
{
    protected override MySqlEventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<MySqlEventSourcingDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySql(
            Configuration.GetConnectionString("MySqlConnection"),
            new MySqlServerVersion(new Version(8, 2, 0))
        );

        return new MySqlEventSourcingDbContext(optionsBuilder.Options);
    }
}
