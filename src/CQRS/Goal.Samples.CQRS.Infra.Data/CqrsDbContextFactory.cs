using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Goal.Samples.CQRS.Infra.Data;

public class CqrsDbContextFactory : DesignTimeDbContextFactory<CqrsDbContext>
{
    protected override CqrsDbContext CreateNewInstance(DbContextOptionsBuilder<CqrsDbContext> optionsBuilder)
    {
        string connectionString = Configuration.GetConnectionString("DefaultConnection");
        string dbProvider = Configuration.GetValue("DbProvider", "SqlServer");
        string migrationsAssembly = typeof(CqrsDbContext).Assembly.GetName().Name;

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

        return new CqrsDbContext(optionsBuilder.Options);
    }
}
