using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetFirstDemo.Model;
using NetFirstDemo.Model.authentication;

namespace NetFirstDemo.Service.authentication
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(LoginRequestDTO request);

        public UserDetail GetUser(String username);

        public string GenerateToken(string username, TokenManagement tokenManagement)
        {
            UserDetail user = GetUser(username);
            if (user.Locked == true) {
                return "locked";
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            user.Roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                tokenManagement.Issuer,
                tokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
