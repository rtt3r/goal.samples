using System.Reflection;
using Goal.Seedwork.Infra.Http.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.Infra.Http.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public virtual void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = GetApiTitle(), Version = "v1" });
        options.IncludeXmlComments(GetXmlCommentsFile());
        options.DocumentFilter<LowerCaseDocumentFilter>();
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        options.OperationFilter<SnakeCaseQueryOperationFilter>();
    }

    private static string GetApiTitle()
        => Assembly.GetEntryAssembly().GetName().Name.Replace(".", " ");

    private static string GetXmlCommentsFile()
        => Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");
}
