using System.Text.Json;
using Goal.Demo2.Infra.Extensions;

namespace Goal.Demo2.Infra.Support
{
    public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
}
