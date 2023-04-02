using Goal.Samples.CQRS.Application.Bus.Consumers;
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
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.DependencyInjection;

namespace Goal.Samples.CQRS.Infra.IoC.Extensions
{
    public static class DependencyInjectionMethods
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);
            services.AddScoped<AppState>();

            services.AddAutoMapperTypeAdapter();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddPublisherMassTransit(configuration);

            services.AddSampleDbContext(connectionString);
            services.AddRavenDb(configuration);
            services.AddScoped<ISampleUnitOfWork, SampleUnitOfWork>();
            services.AddDefaultNotificationHandler();

            services.RegisterAllTypesOf<IRepository>(typeof(CustomerRepository).Assembly);
            services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

            return services;
        }

        public static IServiceCollection ConfigureWorkerServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddEventSourcingDbContext(connectionString);
            services.AddRavenDb(configuration);
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddDefaultNotificationHandler();
            services.AddConsumerMassTransit(configuration);

            services.RegisterAllTypesOf<IQueryRepository>(typeof(CustomerQueryRepository).Assembly);

            return services;
        }

        private static IServiceCollection AddConsumerMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.AddConsumer<CustomerRegisteredEventConsumer>(typeof(CustomerRegisteredEventConsumer.ConsumerDefinition));
                x.AddConsumer<CustomerRemovedEventConsumer>(typeof(CustomerRemovedEventConsumer.ConsumerDefinition));
                x.AddConsumer<CustomerUpdatedEventConsumer>(typeof(CustomerUpdatedEventConsumer.ConsumerDefinition));

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));
                    cfg.UseDelayedMessageScheduler();
                    cfg.ServiceInstance(instance =>
                    {
                        instance.ConfigureJobServiceEndpoints();
                        instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    });
                });
            });

            return services;
        }

        private static IServiceCollection AddPublisherMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));

                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
                });
            });

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
                .AddDbContext<SampleDbContext>((provider, options) =>
                {
                    options
                        .UseSqlServer(
                            connectionString,
                            opts =>
                            {
                                opts.MigrationsAssembly(typeof(SampleDbContext).Assembly.GetName().Name);
                                opts.EnableRetryOnFailure();
                            })
                        .EnableSensitiveDataLogging()
                        ;
                });

            return services;
        }

        private static IServiceCollection AddEventSourcingDbContext(this IServiceCollection services, string connectionString)
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
