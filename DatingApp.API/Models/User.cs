namespace DatingApp.API.Models
{
    public class User
    {

        // Getters & Setters
        public int Id {get; set;}               // Id

        public string Username {get; set;}      // Username

        public byte[] PasswordHash {get; set;}  // PasswordHash

        public byte[] PasswordSalt {get; set;}  // PassWordSalt

    }   //End of User class
}   //End of namespace