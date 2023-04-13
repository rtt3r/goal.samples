using System.Security.Claims;
using Goal.Samples.Infra.Crosscutting.Extensions;

namespace Goal.Samples.Infra.Crosscutting;

public sealed class AppUser
{
    public AppUser(ClaimsPrincipal principal)
    {
        Email = principal.GetEmail();
        UserName = principal.GetUserName();
        GivenName = principal.GetGivenName();
        UserId = principal.GetUserId();
        ClientId = principal.GetClientId();
        Roles = principal.GetRoles();
    }

    public string UserId { get; protected set; }
    public string Email { get; protected set; }
    public string UserName { get; set; }
    public string GivenName { get; protected set; }
    public string ClientId { get; set; }
    public IEnumerable<string> Roles { get; protected set; }
}
