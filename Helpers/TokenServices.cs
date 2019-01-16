using JWT.Builder;
using JWT.Algorithms;
using System;
namespace ShopAPI.Helpers
{
    public class TokenServices
    {
        private static string SECRET = "3D_HungPT";
        private static string TOKEN_PREFIX = "Bearer ";
        public static string GetTokenFromUser(string username)
        {
            var tokenHandler = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
                                .WithSecret(SECRET).Subject(username).AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds()).Build();
            return TOKEN_PREFIX + tokenHandler;
        }

        
    }
}