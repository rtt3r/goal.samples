using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Goal.Samples.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources
            => new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources
            => new List<ApiResource>
            {
                new ApiResource("goal", "Goal API")
                {
                    Scopes = { "goal.read", "goal.write", "manage" }
                },
            };

        public static IEnumerable<ApiScope> ApiScopes
            => new List<ApiScope>
            {
                new ApiScope("goal.read"),
                new ApiScope("goal.write"),
                new ApiScope("manage")
            };

        public static IEnumerable<Client> Clients
            => new Client[]
            {
                new Client
                {
                    Enabled = true,
                    ClientId = "goal.web",
                    ClientName = "Goal Web",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 360000,
                    SlidingRefreshTokenLifetime = 36000,
                    ClientClaimsPrefix = string.Empty,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "goal.read",
                        "goal.write",
                        "manage"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200",
                        "https://localhost:4200",
                        "http://localhost:8002"
                    }
                },
            };
    }
}