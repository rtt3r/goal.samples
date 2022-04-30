using System.Security.Claims;

namespace Goal.Demo2.Infra.Crosscutting
{
    public sealed class AppState
    {
        public AppState(ClaimsPrincipal principal)
        {
            User = new AppUser(principal);
        }

        public AppUser User { get; set; }
    }
}
