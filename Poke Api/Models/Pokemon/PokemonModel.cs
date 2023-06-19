using Poke_Api.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Poke_Api.Models.Pokemon
{
    public class PokemonModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Pokemon id is required")]
        public int idPokemon { get; set;}
        [Required(ErrorMessage = "Pokemon name id is required")]
        [MaxLength(150)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Pokemon url is required")]
        [MaxLength(255)]    
        public string? Url { get; set; }
        [JsonIgnore]
        public ICollection<UserModel>? UserFavorited { get; set; }    
    }
}
