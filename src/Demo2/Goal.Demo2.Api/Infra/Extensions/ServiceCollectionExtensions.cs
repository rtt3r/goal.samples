using Goal.Demo2.Api.Infra.TypeAdapters;
using Goal.Infra.Http.Seedwork.Extensions;

namespace Goal.Demo2.Api.Infra.Extensions
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
