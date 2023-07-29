using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.MySQL;

public class MySQLEventSourcingDbContextFactory : DesignTimeDbContextFactory<MySQLEventSourcingDbContext>
{
    protected override MySQLEventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<MySQLEventSourcingDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySQL(Configuration.GetConnectionString("MySQLConnection"));
        return new MySQLEventSourcingDbContext(optionsBuilder.Options);
    }
}
