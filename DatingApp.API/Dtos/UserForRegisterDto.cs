/*
* This is going to be a class specific for data transfering user information
*/

using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    
    public class UserForRegisterDto         // Constructor
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage= "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}   // End of UserForRigesterDto class