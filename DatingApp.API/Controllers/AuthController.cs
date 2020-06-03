/*
* 
*/

using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;

        // Constructor
        public AuthController(IAuthRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto )
        {
            // Validate the request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();        // Save the username as lowercasea

            // Check if username is already taken
            if (await repo.UserExists(userForRegisterDto.Username))                     // If the username exists already
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User 
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); //This will be changed to return the user
        }
    }
}