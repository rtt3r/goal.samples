using Goal.Samples.CQRS.Application.TypeAdapters;
using Goal.Samples.CQRS.Infra.Data;
using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.CQRS.Infra.Data.Repositories;
using Goal.Samples.Infra.Crosscutting;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Domain.Events;
using Goal.Seedwork.Infra.Data.Query;
using Goal.Seedwork.Infra.Http.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.DependencyInjection;

namespace Goal.Samples.CQRS.Infra.IoC.Extensions;

public static class ServiceColletionExtensionMethods
{
    public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);
        services.AddScoped<AppState>();

        services.AddAutoMapperTypeAdapter();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddDefaultNotificationHandler();
        services.AddRavenDb(configuration);

        services.AddSampleDbContext(configuration);
        services.AddScoped<ICqrsUnitOfWork, CqrsUnitOfWork>();
        services.RegisterAllTypesOf<IRepository>(typeof(CustomerRepository).Assembly);
        services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

        return services;
    }

    public static IServiceCollection ConfigureWorkerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);
        services.AddScoped<AppState>();

        services.AddAutoMapperTypeAdapter();

        services.AddDefaultNotificationHandler();
        services.AddRavenDb(configuration);

        services.AddEventSourcingDbContext(configuration);
        services.AddScoped<IEventStore, SqlEventStore>();

        services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

        return services;
    }

    private static IServiceCollection AddAutoMapperTypeAdapter(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperAdapterFactory).Assembly);
        services.AddTypeAdapterFactory<AutoMapperAdapterFactory>();

        return services;
    }

    private static IServiceCollection AddSampleDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        string dbProvider = configuration.GetValue("DbProvider", "SqlServer");
        string migrationsAssembly = typeof(CqrsDbContext).Assembly.GetName().Name;

        services
            .AddDbContext<CqrsDbContext>((provider, options) =>
            {
                DbContextOptionsBuilder builder = dbProvider switch
                {
                    "MySQL" => options.UseMySQL(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.MySQL")),

                    "SqlServer" => options.UseSqlServer(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.SqlServer")),

                    "Npgsql" => options.UseNpgsql(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.Npgsql")),

                    _ => throw new Exception($"Unsupported provider: {dbProvider}")
                };

                builder.EnableSensitiveDataLogging();
            });

        return services;
    }

    private static IServiceCollection AddEventSourcingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        string dbProvider = configuration.GetValue("DbProvider", "SqlServer");
        string migrationsAssembly = typeof(EventSourcingDbContext).Assembly.GetName().Name;

        services
            .AddDbContext<EventSourcingDbContext>((provider, options) =>
            {
                DbContextOptionsBuilder builder = dbProvider switch
                {
                    "MySQL" => options.UseMySQL(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.MySQL")),

                    "SqlServer" => options.UseSqlServer(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.SqlServer")),

                    "Npgsql" => options.UseNpgsql(
                        connectionString,
                        x => x.MigrationsAssembly($"{migrationsAssembly}.Npgsql")),

                    _ => throw new Exception($"Unsupported provider: {dbProvider}")
                };

                builder.EnableSensitiveDataLogging();
            });

        return services;
    }

    private static IServiceCollection AddRavenDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RavenSettings>(configuration.GetSection("RavenSettings"));

        services.AddRavenDbDocStore(opts =>
        {
            string urls = configuration["RavenSettings:Urls"];
            opts.Settings = new RavenSettings
            {
                Urls = urls.Split(',', StringSplitOptions.RemoveEmptyEntries),
                DatabaseName = configuration["RavenSettings:DatabaseName"],
                CertFilePath = configuration["RavenSettings:CertFilePath"],
                CertPassword = configuration["RavenSettings:CertPassword"],
            };
        });

        services.AddRavenDbAsyncSession();
        services.AddRavenDbSession();

        return services;
    }
}
