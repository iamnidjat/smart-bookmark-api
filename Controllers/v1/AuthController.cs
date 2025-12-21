using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;
using SmartBookmarkApi.ViewModels;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpGet("random-username")]
        public IActionResult GetRandomUsername()
        {
            var username = _authService.GetRandomUsername();
            return Ok(new { username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authResponse = await _authService.LoginAsync(model.Username, model.Password, HttpContext);

            if (authResponse is null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(authResponse);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) // prevents invalid or incomplete data from reaching your service layer and DB
                return BadRequest(ModelState);

            var authResponse = await _authService.SignupAsync(model.Username, model.Password, model.ConfirmPassword, model.Email, HttpContext);

            if (authResponse is null)
            {
                return BadRequest(new { message = "Signup failed." });
            }

            return Created("", authResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                var newAccessToken = await _authService.RefreshAsync(HttpContext);

                return Ok(newAccessToken);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Server error" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(int userId)
        {
            var logoutResponse = await _authService.LogoutAsync(userId);

            if (logoutResponse is null)
            {
                return BadRequest(new { message = "Logout failed." });
            }

            return Ok();
        }
    }
}
