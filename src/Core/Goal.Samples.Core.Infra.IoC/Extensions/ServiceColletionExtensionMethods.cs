using Goal.Samples.Core.Application.TypeAdapters;
using Goal.Samples.Core.Infra.Data;
using Goal.Samples.Core.Infra.Data.EventSourcing;
using Goal.Samples.Core.Infra.Data.MySQL;
using Goal.Samples.Core.Infra.Data.Npgsql;
using Goal.Samples.Core.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.Core.Infra.Data.Repositories;
using Goal.Samples.Core.Infra.Data.SqlServer;
using Goal.Samples.Infra.Crosscutting;
using Goal.Samples.Infra.Crosscutting.Settings;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Domain.Events;
using Goal.Seedwork.Infra.Data.Query;
using Goal.Seedwork.Infra.Http.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.DependencyInjection;

namespace Goal.Samples.Core.Infra.IoC.Extensions;

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

        services.AddCoreDbContext(configuration);
        services.AddScoped<ICoreUnitOfWork, CoreUnitOfWork>();
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

    public static IServiceCollection AddKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        KeycloakSettings keycloakOptions = configuration
            .GetSection(KeycloakSettings.Section)
            .Get<KeycloakSettings>();

        services.AddKeycloakAuthentication(keycloakOptions.AuthenticationOptions);
        services.AddKeycloakAuthorization(keycloakOptions.ProtectionClientOptions);
        services.AddKeycloakAdminHttpClient(keycloakOptions.AdminClientOptions);

        return services;
    }

    private static IServiceCollection AddAutoMapperTypeAdapter(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperAdapterFactory).Assembly);
        services.AddTypeAdapterFactory<AutoMapperAdapterFactory>();

        return services;
    }

    private static IServiceCollection AddCoreDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string dbProvider = configuration.GetValue("DbProvider", "SqlServer");
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        if (dbProvider == "SqlServer")
        {
            services.AddDbContext<CoreDbContext, SqlServerCoreDbContext>((provider, options) =>
            {
                options
                    .UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(SqlServerCoreDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else if (dbProvider == "MySQL")
        {
            services.AddDbContext<CoreDbContext, MySQLCoreDbContext>((provider, options) =>
            {
                options
                    .UseMySQL(connectionString, x => x.MigrationsAssembly(typeof(MySQLCoreDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else if (dbProvider == "Npgsql")
        {
            services.AddDbContext<CoreDbContext, NpgsqlCoreDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(NpgsqlCoreDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else
        {
            throw new Exception($"Unsupported provider: {dbProvider}");
        }

        return services;
    }

    private static IServiceCollection AddEventSourcingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string dbProvider = configuration.GetValue("DbProvider", "SqlServer");
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        if (dbProvider == "SqlServer")
        {
            services.AddDbContext<EventSourcingDbContext, SqlServerEventSourcingDbContext>((provider, options) =>
            {
                options
                    .UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(SqlServerEventSourcingDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else if (dbProvider == "MySQL")
        {
            services.AddDbContext<EventSourcingDbContext, MySQLEventSourcingDbContext>((provider, options) =>
            {
                options
                    .UseMySQL(connectionString, x => x.MigrationsAssembly(typeof(MySQLEventSourcingDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else if (dbProvider == "Npgsql")
        {
            services.AddDbContext<EventSourcingDbContext, NpgsqlEventSourcingDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(connectionString, x => x.MigrationsAssembly(typeof(NpgsqlEventSourcingDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging();
            });
        }
        else
        {
            throw new Exception($"Unsupported provider: {dbProvider}");
        }

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
