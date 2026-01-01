using System.Security.Claims;

namespace SmartBookmarkApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // we get here the userId from the JWT token
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("UserId missing")
            );
        }
    }
}
