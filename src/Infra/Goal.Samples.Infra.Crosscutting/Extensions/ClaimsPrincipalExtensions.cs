using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;

namespace Goal.Samples.Infra.Crosscutting.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetClaimValue(this ClaimsPrincipal principal, params string[] claimTypes)
        => principal?.GetClaimValues(claimTypes).FirstOrDefault();

    public static IEnumerable<string> GetClaimValues(this ClaimsPrincipal principal, params string[] claimTypes)
    {
        var claimValues = principal
            .Claims
            .Where(p => claimTypes.Contains(p.Type))
            .Select(p => p.Value)
            .ToList();

        return claimValues;
    }

    public static Guid GetClaimValueAsGuid(this ClaimsPrincipal principal, string claimName)
    {
        string claimValue = principal.GetClaimValue(claimName);

        if (string.IsNullOrEmpty(claimValue))
        {
            return Guid.Empty;
        }

        return Guid.TryParse(claimValue, out Guid value)
            ? value
            : Guid.Empty;
    }

    public static T GetClaimValueAs<T>(this ClaimsPrincipal principal, string claimName)
        where T : struct
    {
        if (principal is null)
        {
            return default;
        }

        string claimValue = principal.GetClaimValue(claimName);

        if (claimValue is null)
        {
            return default;
        }

        try
        {
            return (T)Convert.ChangeType(claimValue, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    public static string GetUserId(this ClaimsPrincipal principal)
        => principal.GetClaimValue(JwtClaimTypes.Subject, ClaimTypes.NameIdentifier);

    public static string GetEmail(this ClaimsPrincipal principal)
        => principal?.GetClaimValue(JwtClaimTypes.Email, ClaimTypes.Email);

    public static string GetUserName(this ClaimsPrincipal principal)
        => principal?.GetClaimValue(JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, ClaimTypes.Name);

    public static string GetGivenName(this ClaimsPrincipal principal)
        => principal?.GetClaimValue(JwtClaimTypes.GivenName, ClaimTypes.GivenName);

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal)
        => principal?.GetClaimValues(JwtClaimTypes.Role, ClaimTypes.Role);

    public static IEnumerable<string> GetScopes(this ClaimsPrincipal principal)
        => principal?.GetClaimValues(JwtClaimTypes.Scope);

    public static string GetClientId(this ClaimsPrincipal principal)
        => principal?.GetClaimValue(JwtClaimTypes.ClientId);

    public static void AddClaim(this IIdentity identity, Claim claim)
    {
        if (identity is ClaimsIdentity claimsIdentity)
        {
            claimsIdentity.AddClaim(claim);
        }
    }
}
