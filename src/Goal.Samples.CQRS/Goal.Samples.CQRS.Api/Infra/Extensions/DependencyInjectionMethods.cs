using Goal.Samples.CQRS.Api.Application.Bus.Customers;
using Goal.Samples.CQRS.Infra.Crosscutting.Constants;
using Goal.Samples.CQRS.Infra.Data;
using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.CQRS.Infra.Data.Repositories;
using Goal.Samples.Infra.Crosscutting;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Domain.Events;
using Goal.Seedwork.Infra.Data.Query;
using Goal.Seedwork.Infra.Http.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Raven.DependencyInjection;

namespace Goal.Samples.CQRS.Api.Infra.Extensions
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

            services.AddScoped<ISampleUnitOfWork, SampleUnitOfWork>();
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
                .AddDbContext<EventSourcingDbContext>((provider, options) =>
                {
                    options
                        .UseSqlServer(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(EventSourcingDbContext).Assembly.GetName().Name))
                        .EnableSensitiveDataLogging();
                });

            services
                .AddDbContext<SampleDbContext>((provider, options) =>
                {
                    options
                        .UseSqlServer(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(SampleDbContext).Assembly.GetName().Name))
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
