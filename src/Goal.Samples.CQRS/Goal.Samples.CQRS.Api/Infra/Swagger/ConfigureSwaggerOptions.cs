using System.Reflection;
using Goal.Seedwork.Infra.Http.Swagger;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.CQRS.Api.Infra.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Goal Samples CQRS Api", Version = "v1" });
            options.IncludeXmlComments(GetXmlCommentsFile());
            options.DocumentFilter<LowerCaseDocumentFilter>();
            options.DescribeAllParametersInCamelCase();
        }

        private static string GetXmlCommentsFile()
        {
            string xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
    }
}
