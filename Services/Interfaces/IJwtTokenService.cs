namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string username);
        string GenerateRefreshToken();
    }
}
