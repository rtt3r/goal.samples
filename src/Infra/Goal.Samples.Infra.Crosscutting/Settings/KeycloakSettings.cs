using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using Keycloak.AuthServices.Sdk.Admin;

namespace Goal.Samples.Infra.Crosscutting.Settings;

public class KeycloakSettings
{
    public const string Section = ConfigurationConstants.ConfigurationPrefix;

    private KeycloakAdminClientOptions adminClientOptions;
    private KeycloakAuthenticationOptions authenticationOptions;
    private KeycloakProtectionClientOptions protectionClientOptions;

    public string AuthServerUrl { get; set; } = default!;
    public string Realm { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
    public bool? VerifyTokenAudience { get; set; } = default!;
    public KeycloakClientInstallationCredentials Credentials { get; set; } = new();
    public TimeSpan TokenClockSkew { get; set; } = default!;
    public string SslRequired { get; set; } = default!;
    public RolesClaimTransformationSource RolesSource { get; set; } = RolesClaimTransformationSource.ResourceAccess;

    public KeycloakAdminClientOptions AdminClientOptions
        => adminClientOptions ??= new()
        {
            AuthServerUrl = AuthServerUrl,
            Realm = Realm,
            Resource = Resource,
            VerifyTokenAudience = VerifyTokenAudience,
            Credentials = Credentials,
            TokenClockSkew = TokenClockSkew,
            SslRequired = SslRequired,
            RolesSource = RolesSource
        };

    public KeycloakAuthenticationOptions AuthenticationOptions
        => authenticationOptions ??= new()
        {
            AuthServerUrl = AuthServerUrl,
            Realm = Realm,
            Resource = Resource,
            VerifyTokenAudience = VerifyTokenAudience,
            Credentials = Credentials,
            TokenClockSkew = TokenClockSkew,
            SslRequired = SslRequired,
            RolesSource = RolesSource
        };

    public KeycloakProtectionClientOptions ProtectionClientOptions
        => protectionClientOptions ??= new()
        {
            AuthServerUrl = AuthServerUrl,
            Realm = Realm,
            Resource = Resource,
            VerifyTokenAudience = VerifyTokenAudience,
            Credentials = Credentials,
            TokenClockSkew = TokenClockSkew,
            SslRequired = SslRequired,
            RolesSource = RolesSource
        };
}