using System.ComponentModel.DataAnnotations;

namespace Poke_Api.Models.User
{
    public class UserLogin
    {
        [Required(ErrorMessage = "UserName id required")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long")]
        public string? UserName { get; set; }
        [Required( ErrorMessage = "Password id required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string? Password { get; set; }
    }
}
