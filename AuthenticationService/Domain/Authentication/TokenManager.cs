using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Domain.Authentication
{
    public class TokenManager
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }

        public List<Claim> Claims = new();

        public void AddUsernameAsClaim(string username)
        {
            Claims.Add(new Claim(ClaimTypes.Name, username));
        }

        public void AddRolesAsClaim(List<string> roles)
        {
            roles.ForEach(role =>
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            });
        }

        public string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                Issuer,
                Audience,
                Claims,
                expires: DateTime.Now.AddMinutes(AccessExpiration),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
