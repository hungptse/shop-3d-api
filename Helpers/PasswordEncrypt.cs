using BCrypt;
namespace ShopAPI.Helpers
{
    public class PasswordEncrypt
    {

        public static string Encrypt(string rawPassword)
        {
            
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
        
        public static bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}