using Goal.Samples.Infra.Crosscutting.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.Infra.Http.Swagger;

public class SnakeCaseQueryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        foreach (OpenApiParameter item in operation.Parameters)
        {
            item.Name = item.Name.ToSnakeCase();
        }
    }
}
