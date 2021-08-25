using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Controller.Request;
using AuthenticationService.Model;
using Microsoft.IdentityModel.Tokens;

namespace NetFirstDemo.Service.authentication
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(LoginRequest request);

        public UserDetail GetUser(String username);

        public string GenerateToken(string username, TokenConfig tokenConfig)
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                tokenConfig.Issuer,
                tokenConfig.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(tokenConfig.AccessExpiration),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
