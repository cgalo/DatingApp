/*
* 
*/

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        // Constructor
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
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
        }   // End of Register

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // First check if user exists, passing the username as lowercase
            var userFromRepo = await repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)   // User was not found in DB
                return Unauthorized();  // Return a 401 http unauthorized request

            // Build a token to return to the user, will contain user's ID & the user's username
            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),  // ID
                new Claim (ClaimTypes.Name, userFromRepo.Username)                  // Username
            };

            // Now we need to handle signing the token for securit
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Security token descriptor, start of the creation of the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),      // Token expires in 24 hours (1 day)
                SigningCredentials = creds
            };

            // Token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token as an object to our client
            return Ok(new {
                token = tokenHandler.WriteToken(token)  
            });

        }   // End of Login method
}
}