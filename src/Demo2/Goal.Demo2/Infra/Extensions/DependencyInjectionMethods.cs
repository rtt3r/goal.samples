using Goal.Demo2.Application.Bus.Customers;
using Goal.Demo2.Infra.Crosscutting;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Infra.Data;
using Goal.Demo2.Infra.Data.EventSourcing;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Demo2.Infra.Data.Repositories;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Domain.Events;
using Goal.Seedwork.Infra.Data.Query;
using Goal.Seedwork.Infra.Http.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Raven.DependencyInjection;

namespace Goal.Demo2.Infra.Extensions
{
    public static class DependencyInjectionMethods
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);
            services.AddScoped<AppState>();

            services.AddDbContexts(connectionString);
            services.AddRavenDb(configuration);

            services.AddScoped<IDemo2UnitOfWork, Demo2UnitOfWork>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddDefaultNotificationHandler();

            services.AddMassTransit(config =>
            {
                config.AddConsumer<CustomerBusConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint(ApplicationConstants.EventBus.CustomerRegisteredQueue, c =>
                    {
                        c.ConfigureConsumer<CustomerBusConsumer>(ctx);
                    });

                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.RegisterAllTypesOf<IRepository>(typeof(CustomerRepository).Assembly);
            services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

            return services;
        }

        private static IServiceCollection AddDbContexts(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<EventSourcingContext>((provider, options) =>
                {
                    options
                        .UseSqlServer(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(EventSourcingContext).Assembly.GetName().Name))
                        .EnableSensitiveDataLogging();
                });

            services
                .AddDbContext<Demo2Context>((provider, options) =>
                {
                    options
                        .UseSqlServer(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(Demo2Context).Assembly.GetName().Name))
                        .EnableSensitiveDataLogging();
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
}
