using Goal.Samples.CQRS.Infra.TypeAdapters;
using Goal.Seedwork.Infra.Http.DependencyInjection;

namespace Goal.Samples.CQRS.Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperTypeAdapter(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperAdapterFactory).Assembly);
            services.AddTypeAdapterFactory<AutoMapperAdapterFactory>();

            return services;
        }
    }
}
