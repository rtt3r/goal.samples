using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Ritter.Starter.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("starter", "Starter API")
                {
                    Scopes = { "starter.read", "starter.write", "manage" }
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("starter.read"),
                new ApiScope("starter.write"),
                new ApiScope("manage")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    Enabled = true,
                    ClientId = "starter.web",
                    ClientName = "Starter Web",
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
                        "starter.read",
                        "starter.write",
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