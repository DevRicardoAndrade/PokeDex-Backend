using Poke_Api.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Poke_Api.Models.Rules
{
    public class RuleModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }
        [JsonIgnore]
        public IEnumerable<UserModel>? Users { get; set; }
    }
}
