using Microsoft.EntityFrameworkCore;
using Poke_Api.Models.Rules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Poke_Api.Models.User
{
    [Index(nameof(Email), nameof(UserName), IsUnique = true)]   
    public class UserModel
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "User name must be no longer than 50 characters"), MinLength(3, ErrorMessage = "User name must be at least 3 characters long")]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Email { get; set; }
        [StringLength(20)]
        public string? Password { get; set; }
        [StringLength(20)]
        public string? UserName { get; set; }
        public IEnumerable<RuleModel>? Rules { get; set; }

    }
}
