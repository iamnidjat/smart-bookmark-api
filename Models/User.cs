using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        public ICollection<Bookmark> Bookmarks = [];

        public ICollection<Category> Categories = [];

        public ICollection<RefreshToken> RefreshTokens = [];
    }
}
