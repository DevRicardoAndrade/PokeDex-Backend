using Poke_Api.Models.Pokemon;

namespace Poke_Api.Repositories.Pokemon
{
    public interface IPokemon
    {
        public Task<PokemonModel> CreateAsync(PokemonModel model);  
        public Task<PokemonModel> UpdateAsync(PokemonModel model, int id);
        public Task<PokemonModel> DeleteAsync(int id);    
        public Task<PokemonModel> GetPokemonAsync(int id);
        public Task<PokemonModel[]> GetPokemonsAsync();
        public Task<PokemonModel[]> GetFavoriteAsync(string token);
        public Task<PokemonModel> SetFavoritePokemons(PokemonModel model, string token);
    }
}
