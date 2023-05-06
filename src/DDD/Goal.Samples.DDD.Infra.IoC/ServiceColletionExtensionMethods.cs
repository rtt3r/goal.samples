using Goal.Samples.DDD.Application.People;
using Goal.Samples.DDD.Application.TypeAdapters;
using Goal.Samples.DDD.Domain;
using Goal.Samples.DDD.Infra.Data;
using Goal.Samples.DDD.Infra.Data.Repositories;
using Goal.Samples.Infra.Crosscutting;
using Goal.Seedwork.Application.Services;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Infra.Http.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Samples.DDD.Infra.IoC;

public static class ServiceColletionExtensionMethods
{
    public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHttpContextAccessor();
        services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);
        services.AddScoped<AppState>();

        services.AddAutoMapperTypeAdapter();

        services.AddSampleDbContext(connectionString);
        services.AddScoped<IDddUnitOfWork, DddUnitOfWork>();
        services.RegisterAllTypesOf<IRepository>(typeof(PeopleRepository).Assembly);
        services.RegisterAllTypesOf<IAppService>(typeof(PersonAppService).Assembly);

        return services;
    }

    private static IServiceCollection AddAutoMapperTypeAdapter(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperAdapterFactory).Assembly);
        services.AddTypeAdapterFactory<AutoMapperAdapterFactory>();

        return services;
    }

    private static IServiceCollection AddSampleDbContext(this IServiceCollection services, string connectionString)
    {
        services
            .AddDbContext<DddDbContext>((provider, options) =>
            {
                options
                    .UseSqlServer(
                        connectionString,
                        opts =>
                        {
                            opts.MigrationsAssembly(typeof(DddDbContext).Assembly.GetName().Name);
                            opts.EnableRetryOnFailure();
                        })
                    .EnableSensitiveDataLogging();
            });

        return services;
    }
}
