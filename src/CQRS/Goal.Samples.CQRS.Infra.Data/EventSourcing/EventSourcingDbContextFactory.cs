using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data.EventSourcing;

public class EventSourcingDbContextFactory : DesignTimeDbContextFactory<EventSourcingDbContext>
{
    protected override EventSourcingDbContext CreateNewInstance(DbContextOptionsBuilder<EventSourcingDbContext> optionsBuilder)
    {
        string connectionString = Configuration.GetConnectionString("DefaultConnection");
        string dbProvider = Configuration.GetValue("DbProvider", "SqlServer");
        string migrationsAssembly = typeof(EventSourcingDbContext).Assembly.GetName().Name;

        optionsBuilder = dbProvider switch
        {
            "MySQL" => optionsBuilder.UseMySQL(
                connectionString,
                x => x.MigrationsAssembly($"{migrationsAssembly}.MySQL")),

            "SqlServer" => optionsBuilder.UseSqlServer(
                connectionString,
                x => x.MigrationsAssembly($"{migrationsAssembly}.SqlServer")),

            "Npgsql" => optionsBuilder.UseNpgsql(
                connectionString,
                x => x.MigrationsAssembly($"{migrationsAssembly}.Npgsql")),

            _ => throw new Exception($"Unsupported provider: {dbProvider}")
        };

        return new EventSourcingDbContext(optionsBuilder.Options);
    }
}
