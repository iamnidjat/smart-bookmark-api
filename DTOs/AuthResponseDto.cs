using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.DTOs
{
    public class AuthResponseDto
    {
        [Required]
        public string Token { get; set; } = "";

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";
    }
}
