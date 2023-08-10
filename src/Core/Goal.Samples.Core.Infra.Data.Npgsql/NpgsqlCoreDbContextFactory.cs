using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.Core.Infra.Data.Npgsql;

public class NpgsqlCoreDbContextFactory : DesignTimeDbContextFactory<NpgsqlCoreDbContext>
{
    protected override NpgsqlCoreDbContext CreateNewInstance(DbContextOptionsBuilder<NpgsqlCoreDbContext> optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection"));
        return new NpgsqlCoreDbContext(optionsBuilder.Options);
    }
}
