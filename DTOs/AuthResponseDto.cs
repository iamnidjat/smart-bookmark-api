using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.DTOs
{
    // Data annotations like [Required] are not needed for read-only DTOs.
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";

        public int UserId { get; set; }

        public string Username { get; set; } = "";

        public string Email { get; set; } = "";
    }
}
