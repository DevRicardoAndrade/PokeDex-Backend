using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poke_Api.Models.User;
using Poke_Api.Repositories.User;
using Poke_Api.Utils;

namespace Poke_Api.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser? _user;
        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                UserModel? userDeleted = await _user.DeleteUserAsync(id);
                if (userDeleted != null)
                {
                    return Ok(userDeleted);
                }
                else
                {
                    return NotFound("User id " + id + " not found!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [JwtAuthorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) 
        {
            try
            {
                UserModel? userSelected = await _user.GetUserAsync(id);
                if (userSelected != null)
                {
                    return Ok(userSelected);
                }
                else
                {
                    return NotFound("User id " + id + " not found!");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [JwtAuthorize]
        [HttpGet("/api/user/me")]
        public async Task<IActionResult> Me()
        {
            try 
            {
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                UserModel? userSelected = await _user.Me(token);
                if (userSelected != null)
                {
                    return Ok(userSelected);
                }
                else
                {
                    return NotFound("User not found!");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [JwtAuthorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<UserModel>? usersSelecteds = await _user.GetUsersAsync();
                if (usersSelecteds.Count() > 0)
                {
                    return Ok(usersSelecteds);
                }
                else
                {
                    return NotFound("Users not found!");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("/api/user/login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            try
            {   
                if(user.Password.Length <=0 || user.UserName.Length<= 0)
                {
                    return BadRequest("UserName or Password is invalid!");
                }

                string token = await _user.LoginAsync(user);

                if(string.IsNullOrEmpty(token))
                {
                    return Unauthorized("User credentials is not found!");
                }
                return Ok(new { Token = token });   
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("/api/user/register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            try
            {
                UserModel? userInserted = await _user.PostUserAsync(user);
                if(userInserted != null)
                {
                    return Ok(userInserted);
                }
                else
                {   
                    return NotFound("Not Found");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNameOrPassword([FromBody] UserModel user, int id)
        {
            try
            {
                UserModel? userUpdated = await _user.PutUserAsync(user, id);
                if (userUpdated != null)
                {
                    return Ok(userUpdated);
                }
                else
                {
                    return NotFound("Not Found");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
