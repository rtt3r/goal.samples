using System.Security.Claims;
using Goal.Samples.Infra.Crosscutting.Extensions;

namespace Goal.Samples.Infra.Crosscutting
{
    public sealed class AppUser
    {
        public AppUser(ClaimsPrincipal principal)
        {
            UserName = principal.GetUserName();
            UserId = principal.GetUserId();
            ClientId = principal.GetClientId();
            Roles = principal.GetRoles();
        }

        public string UserId { get; private set; }
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public IEnumerable<string> Roles { get; private set; }
    }
}
