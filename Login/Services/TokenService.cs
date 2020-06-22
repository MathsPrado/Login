using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Login.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Login.Controllers;
using System.Security.Claims;

namespace Login.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            var tokenHendler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secred);
            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHendler.CreateToken(tokendescriptor);
            return tokenHendler.WriteToken(token);

        }
    }
}
