/**
 * This repository will query our database through our Identity Framework
 * Inject the data content as well
*/

using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context; 
        }
        public async Task<User> Login(string username, string password)
        {
            // Either it returns the username from DB or return null
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            
            // Now we check if the the username exists in the DB
            if (user == null)   // If the we couldn't find the user in the DB
            {
                return null;    // Will return 401 unauthorized
            }
            // We get here if the username exists in the DB
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                // If we the password does not match the one in DB
                return null;    // Will return 401 unauthorized
            }
            // We get here if the input username & passsword matched the one in the DB
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // This should be the same as the one created in the method CreatedPasswordHash
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  //Now compute the hash
                
                // Check that the computedHash and the passwordHash are the same, if not return false
                for (int i = 0; i < computedHash.Length; i++)
                {
                    
                    if (computedHash[i] != passwordHash[i])     // We found a value that is not the same between the two hash values
                        return false;                           // Return false
                }
                // We get here if the computedHash & the passWordHash are the same
                return true;                                    // Return true
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;                                                        // Save the salt key value for this password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  // Now compute the hash
            }
        }

        public async Task<bool> UserExists(string username)
        {
            // If we found a matching username in the DB
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            // Else we didn't find a matching username in the DB
            else
                return false;
        }
    }   // End of interface
}   // End of namespace