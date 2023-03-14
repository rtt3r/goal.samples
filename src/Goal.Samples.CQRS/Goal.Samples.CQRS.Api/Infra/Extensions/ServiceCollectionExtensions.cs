using Goal.Samples.CQRS.Api.Infra.TypeAdapters;
using Goal.Seedwork.Infra.Http.DependencyInjection;

namespace Goal.Samples.CQRS.Api.Infra.Extensions
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
