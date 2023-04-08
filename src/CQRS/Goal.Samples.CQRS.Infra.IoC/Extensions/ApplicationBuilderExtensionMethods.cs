using Goal.Samples.CQRS.Infra.Data;
using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Samples.CQRS.Infra.IoC.Extensions;

public static class ApplicationBuilderExtensionMethods
{
    public static WebApplication MigrateApiDbContext(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<SampleDbContext>().Database.Migrate();
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
