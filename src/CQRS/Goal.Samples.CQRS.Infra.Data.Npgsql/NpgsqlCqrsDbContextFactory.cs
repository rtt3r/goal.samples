using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.Npgsql;

public class NpgsqlCqrsDbContextFactory : DesignTimeDbContextFactory<NpgsqlCqrsDbContext>
{
    protected override NpgsqlCqrsDbContext CreateNewInstance(DbContextOptionsBuilder<NpgsqlCqrsDbContext> optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection"));
        return new NpgsqlCqrsDbContext(optionsBuilder.Options);
    }
}
