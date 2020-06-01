using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        // Register user
        Task <User> Register(User user, string password);
        
        // Log in to API
        Task <User> Login (string username, string password);
        
        // Check if User exists 
        Task <bool> UserExists (string username);
    }   // End of IAuthRepository interface
}   //End of namespace