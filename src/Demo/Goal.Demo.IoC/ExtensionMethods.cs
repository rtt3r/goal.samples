using Goal.Application.Seedwork.Services;
using Goal.Demo.Application.People;
using Goal.Demo.Infra.Data;
using Goal.Demo.Infra.Data.Repositories;
using Goal.Domain.Seedwork;
using Goal.Domain.Seedwork.Aggregates;
using Goal.Infra.Http.Seedwork.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goal.Demo.IoC
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            //services.AddScoped<ElasticAuditChangesInterceptor>();

            services
                .AddDbContext<DemoContext>((provider, options) =>
                {
                    options
                        .UseSqlite(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(DemoContext).Assembly.GetName().Name))
                        .EnableSensitiveDataLogging();

                    //options.AddInterceptors(provider.GetRequiredService<ElasticAuditChangesInterceptor>());
                });

            services.AddScoped<DbContext>(provider => provider.GetService<DemoContext>());

            services.AddScoped<IUnitOfWork, DemoUnitOfWork>();

            services.RegisterAllTypesOf<IRepository>(typeof(PersonRepository).Assembly);
            services.RegisterAllTypesOf<IAppService>(typeof(PersonAppService).Assembly);

            return services;
        }
    }
}
