using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.DDD.Infra.Data;

public class DddDbContextFactory : DesignTimeDbContextFactory<DddDbContext>
{
    protected override DddDbContext CreateNewInstance(DbContextOptionsBuilder<DddDbContext> optionsBuilder, string connectionString)
    {
        optionsBuilder.UseSqlServer(connectionString);
        return new DddDbContext(optionsBuilder.Options);
    }
}
