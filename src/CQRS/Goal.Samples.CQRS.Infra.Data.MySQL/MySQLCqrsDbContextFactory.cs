using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.MySQL;

public class MySQLCqrsDbContextFactory : DesignTimeDbContextFactory<MySQLCqrsDbContext>
{
    protected override MySQLCqrsDbContext CreateNewInstance(DbContextOptionsBuilder<MySQLCqrsDbContext> optionsBuilder)
    {
        optionsBuilder.UseMySQL(Configuration.GetConnectionString("MySQLConnection"));
        return new MySQLCqrsDbContext(optionsBuilder.Options);
    }
}
