using Goal.Samples.Core.Infra.Data;
using Goal.Samples.Core.Infra.Data.EventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Samples.Core.Infra.IoC.Extensions;

public static class ApplicationBuilderExtensionMethods
{
    public static WebApplication MigrateApiDbContext(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<CqrsDbContext>().Database.Migrate();
        }

        return app;
    }

    public static WebApplication MigrateWorkerDbContext(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<EventSourcingDbContext>().Database.Migrate();
        }

        return app;
    }
}
