using Goal.Application.Seedwork.Handlers;
using Goal.Demo2.Api.Infra.Bus;
using Goal.Demo2.Infra.Data;
using Goal.Demo2.Infra.Data.Auditing;
using Goal.Demo2.Infra.Data.EventSourcing;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Demo2.Infra.Data.Repositories;
using Goal.Domain.Seedwork;
using Goal.Domain.Seedwork.Aggregates;
using Goal.Domain.Seedwork.Events;
using Goal.Infra.Data.Query.Seedwork;
using Goal.Infra.Http.Seedwork.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Raven.DependencyInjection;

namespace Goal.Demo2.Api.Infra.Extensions
{
    public static class DependencyInjectionMethods
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            services.AddScoped<Demo2AuditChangesInterceptor>();

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

                    options.AddInterceptors(provider.GetRequiredService<Demo2AuditChangesInterceptor>());
                });

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

            services.AddScoped<IBusHandler, InMemoryBusHandler>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<INotificationHandler, NotificationHandler>();
            services.AddScoped<IUnitOfWork, Demo2UnitOfWork>();

            services.RegisterAllTypesOf<IRepository>(typeof(CustomerRepository).Assembly);
            services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

            return services;
        }
    }
}
