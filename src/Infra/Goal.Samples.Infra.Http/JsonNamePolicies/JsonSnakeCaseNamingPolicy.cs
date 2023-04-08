using System.Text.Json;
using Goal.Samples.Infra.Crosscutting.Extensions;

namespace Goal.Samples.Infra.Http.JsonNamePolicies;

public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
        => name.ToSnakeCase();
}
