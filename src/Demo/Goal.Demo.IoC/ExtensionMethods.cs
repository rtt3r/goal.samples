using Goal.Demo.Application.People;
using Goal.Demo.Infra.Data;
using Goal.Demo.Infra.Data.Repositories;
using Goal.Seedwork.Application.Services;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Domain.Aggregates;
using Goal.Seedwork.Infra.Http.DependencyInjection;
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

            services
                .AddDbContext<DemoContext>((provider, options) =>
                {
                    options
                        .UseSqlite(
                            connectionString,
                            opts => opts.MigrationsAssembly(typeof(DemoContext).Assembly.GetName().Name))
                        .EnableSensitiveDataLogging();
                });

            services.AddScoped<DbContext>(provider => provider.GetService<DemoContext>());

            services.AddScoped<IUnitOfWork, DemoUnitOfWork>();

            services.RegisterAllTypesOf<IRepository>(typeof(PersonRepository).Assembly);
            services.RegisterAllTypesOf<IAppService>(typeof(PersonAppService).Assembly);

            return services;
        }
    }
}
