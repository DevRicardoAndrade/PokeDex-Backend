using Poke_Api.Services;
using Microsoft.EntityFrameworkCore;
using Poke_Api.Context;
using Poke_Api.Models.Pokemon;
using Poke_Api.Models.User;
using System.Data;
using Microsoft.Data.SqlClient;
using Poke_Api.Repositories.User;

namespace Poke_Api.Repositories.Pokemon
{
    public class Pokemon : IPokemon
    {
        private readonly DataContext _context;
        private readonly AuthenticationService _auth;
        public Pokemon(DataContext context, AuthenticationService auth)
        {
            _context = context; 
            _auth = auth;
        }
        public async Task<PokemonModel> CreateAsync(PokemonModel model)
        {
            try
            {
                if(model == null) 
                    throw new ArgumentNullException(nameof(model)); 

                _context.Pokemons.Add(model);   
                await _context.SaveChangesAsync();

                return model;   
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<PokemonModel> DeleteAsync(int id)
        {
            try
            {
                PokemonModel model = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);     
                if (model == null)
                    throw new ArgumentNullException(nameof(model));

                _context.Pokemons.Remove(model);
                await _context.SaveChangesAsync();

                return model;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public async Task<PokemonModel> SetFavoritePokemons(PokemonModel model, string token)
        {
            try
            {
                int UserId = _auth.IdByToken(token);
                PokemonModel pokemonExisting = await _context.Pokemons.FirstOrDefaultAsync(p => p.idPokemon == model.idPokemon);
                if(pokemonExisting == null)
                {
                    _context.Pokemons.Add(model);   
                    await _context.SaveChangesAsync();
                }
                pokemonExisting = await _context.Pokemons.FirstOrDefaultAsync(p => p.idPokemon == model.idPokemon);
                string sqlQuery = "INSERT INTO PokemonModelUserModel (PokemonFavoritedId, UserFavoritedId) VALUES (@Pokemon, @User)";
                await _context.Database.ExecuteSqlRawAsync(sqlQuery, new[] {
                            new SqlParameter("@User", UserId),
                            new SqlParameter("@Pokemon", pokemonExisting.Id)
                        });
                return pokemonExisting; 
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public async Task<PokemonModel[]> GetFavoriteAsync(string token)
        {
            try
            {
                int UserId = _auth.IdByToken(token);
                List<PokemonModel> pokemonsReturn = new List<PokemonModel>();
                List<PokemonModel> pokemons = await _context.Pokemons.Include(p =>p.UserFavorited).ToListAsync();
                pokemons.ForEach(p =>
                {
                    p.UserFavorited.ToList().ForEach(u =>
                    {
                        if(u.Id == UserId)
                        {
                            pokemonsReturn.Add(p);
                        }
                    });
                });
                return pokemons.ToArray();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<PokemonModel> GetPokemonAsync(int id)
        {
            try
            {
                PokemonModel model = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);
                if (model == null)
                    throw new ArgumentNullException(nameof(model));


                return model;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<PokemonModel[]> GetPokemonsAsync()
        {
            try
            {
                PokemonModel[] models = await _context.Pokemons.ToArrayAsync();
                if (models == null)
                    throw new ArgumentNullException(nameof(models));


                return models;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<PokemonModel> UpdateAsync(PokemonModel model, int id)
        {
            try
            {
                PokemonModel modelUpdate = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);
                if (modelUpdate == null)
                    throw new ArgumentNullException(nameof(modelUpdate));

                _context.Pokemons.Update(modelUpdate);
                await _context.SaveChangesAsync();  

                return modelUpdate;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
