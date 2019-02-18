using JWT.Builder;
using JWT.Algorithms;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ShopAPI.Helpers
{
    public class TokenServices
    {
        private static string SECRET = "3D_MODEL_HUNGPT_FUHCM";
        private static string TOKEN_PREFIX = "Bearer ";
        private static string ISSUER = "3dmodel.com";
        private static string AUDIENCE = "3dmodel.com";

        public static string GetTokenFromUser(string username)
        {
            var claimsData = new[] { new Claim("sub", username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenString = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    expires: DateTime.Now.AddDays(1),
                    claims: claimsData,
                    signingCredentials: signInCred
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenString);
            return TOKEN_PREFIX + token;
        }
    }
}