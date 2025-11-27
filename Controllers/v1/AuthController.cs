using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.ViewModels;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpGet("random-username")]
        public string GetRandomUsername()
        {
            return _authService.GetRandomUsername();
        }

        [HttpPost("login")]
        public async Task<IActionResult> StudentLoginAsync([FromBody] LoginModel model)
        {
            var authResponse = await _authService.LoginAsync(model.Username, model.Password, HttpContext);

            if (authResponse is null)
            {
                return StatusCode(500, new { message = "Invalid username or password." });
            }

            return StatusCode(201, authResponse);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> StudentSignupAsync([FromBody] RegisterModel model)
        {
            var authResponse = await _authService.SignupAsync(model.Username, model.Password, model.ConfirmPassword, model.Email, HttpContext);

            if (authResponse is null)
            {
                return StatusCode(500, new { message = "Signup failed." });
            }

            return StatusCode(201, authResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                var newAccessToken = await _authService.RefreshAsync(HttpContext);

                return Ok(newAccessToken);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error" });
            }
        }
    }
}
