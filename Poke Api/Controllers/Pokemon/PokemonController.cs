using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Poke_Api.Models.Pokemon;
using Poke_Api.Models.User;
using Poke_Api.Repositories.Pokemon;

namespace Poke_Api.Controllers.Pokemon
{
    [Route("api/pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemon? _pokemon;
        public PokemonController(IPokemon pokemon)
        {
                _pokemon = pokemon;    
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                PokemonModel model = await _pokemon.GetPokemonAsync(id);
                if(model == null)
                {
                    return NotFound(id + " Not Found");  
                }
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);   
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                PokemonModel[] model = await _pokemon.GetPokemonsAsync();
                if (model == null)
                {
                    return NotFound("Pokemons Not Found");
                }
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PokemonModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                PokemonModel pokemon = await _pokemon.CreateAsync(model);

                if (pokemon == null)
                    return BadRequest("Error");

                return Ok(pokemon); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           

        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Post(int id, [FromBody]PokemonModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                PokemonModel pokemon = await _pokemon.UpdateAsync(model, id);

                if (pokemon == null)
                    return BadRequest("Error");

                return Ok(pokemon);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
        [JwtAuthorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                PokemonModel? model = await _pokemon.DeleteAsync(id);
                if (model != null)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound("Pokemon id " + id + " not found!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [JwtAuthorize]
        [HttpPost("/favorite")]
        public async Task<IActionResult> Favorite([FromBody] PokemonModel model)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                PokemonModel? pokemonFavorited = await _pokemon.SetFavoritePokemons(model, token);
                if (pokemonFavorited != null)
                {
                    return Ok(pokemonFavorited);
                }
                else
                {
                    return NotFound("Error");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [JwtAuthorize]
        [HttpGet("/favorite")]
        public async Task<IActionResult> Favorite()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                PokemonModel[]? pokemonFavorited = await _pokemon.GetFavoriteAsync( token);
                if (pokemonFavorited != null)
                {
                    return Ok(pokemonFavorited);
                }
                else
                {
                    return NotFound("Error");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
