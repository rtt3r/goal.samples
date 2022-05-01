using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;

namespace Goal.Demo2.Infra.Crosscutting.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string claimName)
            => principal?.GetClaimValues(claimName).FirstOrDefault();

        public static IEnumerable<string> GetClaimValues(this ClaimsPrincipal principal, string claimName)
        {
            var claimValues = principal
                .Claims
                .Where(p => p.Type == claimName)
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

            if (Guid.TryParse(claimValue, out Guid value))
            {
                return value;
            }

            return Guid.Empty;
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

        public static string GetUserId(this ClaimsPrincipal principal) => principal.GetClaimValue(JwtClaimTypes.Subject);

        public static string GetUserName(this ClaimsPrincipal principal) => principal?.GetClaimValue(JwtClaimTypes.Name);

        public static string GetUserGivenName(this ClaimsPrincipal principal) => principal?.GetClaimValue(JwtClaimTypes.GivenName);

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal) => principal?.GetClaimValues(JwtClaimTypes.Role);

        public static string GetClientId(this ClaimsPrincipal principal) => principal?.GetClaimValue(JwtClaimTypes.ClientId);

        public static void AddClaim(this IIdentity identity, Claim claim)
        {
            if (identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaim(claim);
            }
        }
    }
}
