using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Interfaces;
using System.Net.Http;
using BC = BCrypt.Net.BCrypt;

namespace SmartBookmarkApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(
          AppDbContext context,
          ILogger<AuthService> logger,
          IJwtTokenService jwtTokenService)
        {
            _context = context;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        public string GetRandomUsername()
        {
            try
            {
                var length = new Random().Next(5, 10);
                return GeneratorService.GenerateCode(length);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Failed to get a random username");
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting a random username");
                return "";
            }
        }

        public async Task<AuthResponseDto?> LoginAsync(string username, string password, HttpContext httpContext)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                if (user == null || !BC.EnhancedVerify(password, user.Password, HashType.SHA512))
                {
                    return null;
                }

                var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Username);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                var tokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false,
                };

                _context.RefreshTokens.Add(tokenEntity);
                await _context.SaveChangesAsync();

                httpContext.Response.Cookies.Append(
                    "refresh_token",
                    refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddDays(7),
                        Path = "/"
                    });

                return new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to log in");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while logging");
                return null;
            }
        }

        public async Task<AuthResponseDto?> SignupAsync(string username, string password, string confirmPassword, string email, HttpContext httpContext)
        {
            try
            {
                if (password != confirmPassword)
                {
                    _logger.LogWarning("Passwords do not match for username: {Username}", username);
                    return null;
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

                if (existingUser != null)
                {
                    _logger.LogWarning($"User {existingUser} already exists");
                    return null;
                }

                var newUser = new User
                {
                    Username = username,
                    Password = BC.EnhancedHashPassword(password, 13, HashType.SHA512),
                    Email = email
                };

                _context.Users.Add(newUser);

                await _context.SaveChangesAsync();

                var token = _jwtTokenService.GenerateToken(newUser.Id.ToString(), newUser.Username);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                var tokenEntity = new RefreshToken
                {
                    UserId = newUser.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false,
                };

                _context.RefreshTokens.Add(tokenEntity);
                await _context.SaveChangesAsync();

                httpContext.Response.Cookies.Append(
                    "refresh_token",
                    refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // true in production
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(7),
                        Path = "/"
                    });

                return new AuthResponseDto
                {
                    Token = token,
                    UserId = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to sign up");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while signing up");
                return null;
            }
        }

        public async Task<string> RefreshAsync(HttpContext httpContext)
        {
            try
            {
                var refreshToken = httpContext.Request.Cookies["refresh_token"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new SecurityTokenException("Refresh token missing");
                }

                var tokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
                
                if (tokenEntity == null)
                {
                    throw new SecurityTokenException("Refresh token not found");
                }

                if (tokenEntity.IsUsed || tokenEntity.ExpiresAt < DateTime.UtcNow)
                {
                    throw new SecurityTokenException("Refresh token is invalid");
                }

                // mark old token as used
                tokenEntity.IsUsed = true;
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(tokenEntity.UserId);
                if (user == null)
                {
                    throw new SecurityTokenException("User not found");
                }

                // generate new pair
                var newAccessToken = _jwtTokenService.GenerateToken((user.Id).ToString(), user.Username);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                // save new refresh token (rotation)
                var newTokenEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false
                };

                _context.RefreshTokens.Add(newTokenEntity);
                await _context.SaveChangesAsync();

                // write new cookie
                httpContext.Response.Cookies.Append(
                    "refresh_token",
                    newRefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // for development
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddDays(7),
                        Path = "/"
                    });

                return newAccessToken;
            }
            catch (Exception ex) when (ex is ArgumentNullException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to refresh the access token");
                throw;
            }
            catch (Exception ex) when (ex is SecurityTokenException)
            {
                _logger.LogError(ex, "Refresh token error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while refreshing the access token");
                throw;
            }
        }
    }
}
