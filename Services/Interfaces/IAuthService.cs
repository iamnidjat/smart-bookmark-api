using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IAuthService
    {
        string GetRandomUsername();
        Task<AuthResponseDto?> LoginAsync(string username, string password, HttpContext httpContext);
        Task<AuthResponseDto?> SignupAsync(string username, string password, string confirmPassword, string email, HttpContext httpContext);
        Task<string> RefreshAsync(HttpContext httpContext);
        Task<OperationResult> LogoutAsync(int userId);
    }
}
