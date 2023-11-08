using Goal.Samples.Core.Infra.Data;
using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Samples.Core.Infra.IoC.Extensions;

public static class ApplicationBuilderExtensionMethods
{
    public static WebApplication MigrateApiDbContext(this WebApplication app)
    {
        try
        {
            using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<CoreDbContext>().Database.Migrate();
            }
        }
        catch
        {
        }

        return app;
    }

    public static WebApplication MigrateWorkerDbContext(this WebApplication app)
    {
        try
        {
            using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<EventSourcingDbContext>().Database.Migrate();
            }
        }
        catch
        {
        }

        return app;
    }
}
