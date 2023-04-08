using System.Security.Claims;
using System.Security.Principal;

namespace UI.Helpers
{
    public static class IdentityExtensions
    {
        public static string GetId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserId");
            return claim?.Value;
        }

        public static string GetNome(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Nome");
            return claim?.Value;
        }

        public static string GetEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Email");
            return claim?.Value;
        }
    }
}
