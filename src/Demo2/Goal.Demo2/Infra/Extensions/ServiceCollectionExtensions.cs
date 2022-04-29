using Goal.Demo2.Infra.TypeAdapters;
using Goal.Seedwork.Infra.Http.Extensions;

namespace Goal.Demo2.Infra.Extensions
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
