using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.Npgsql;

public class NpgsqlEventSourcingDbContextFactory : DesignTimeDbContextFactory<NpgsqlEventSourcingDbContext>
{
    protected override NpgsqlEventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<NpgsqlEventSourcingDbContext> optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection"));
        return new NpgsqlEventSourcingDbContext(optionsBuilder.Options);
    }
}
