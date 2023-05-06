using Goal.Samples.DDD.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Samples.DDD.Infra.IoC;

public static class ApplicationBuilderExtensionMethods
{
    public static WebApplication MigrateApiDbContext(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<DddDbContext>().Database.Migrate();
        }

        return app;
    }
}