using Elasticsearch.Net;
using Goal.Demo2.Api.Infra.TypeAdapters;
using Goal.Infra.Http.Seedwork.Extensions;
using Nest;
using Nest.JsonNetSerializer;

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

        public static IServiceCollection AddElasticClient(this IServiceCollection services, Action<ElasticClientOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            var options = new ElasticClientOptionsBuilder();

            optionsAction?.Invoke(options);

            services.Add(new ServiceDescriptor(
                typeof(ElasticClientOptionsBuilder),
                provider => options,
                optionsLifetime));

            var pool = new SingleNodeConnectionPool(new Uri(options.ConnectionString));
            var connectionSettings = new ConnectionSettings(
                pool,
                sourceSerializer: JsonNetSerializer.Default);

            var client = new ElasticClient(connectionSettings);

            services.Add(new ServiceDescriptor(
                   typeof(ElasticClient),
                   provider => new ElasticClient(connectionSettings),
                   contextLifetime));

            return services;
        }
    }

    public class ElasticClientOptionsBuilder
    {
        public string ConnectionString { get; private set; }

        public void UseConnectionString(string connectionString) => ConnectionString = connectionString;
    }
}
