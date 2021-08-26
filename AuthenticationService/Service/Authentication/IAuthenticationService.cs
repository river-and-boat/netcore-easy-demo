using System;
using UserService.Controller.Request;
using UserService.Domain.Authentication;

namespace UserService.Service.Authentication
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(LoginRequest request);

        public UserDetail GetUser(string username);

        public string GenerateToken(string username, TokenManager tokenManager)
        {
            UserDetail user = GetUser(username);
            user.AssetUserLocked();
            tokenManager.AddUsernameAsClaim(user.Username);
            tokenManager.AddRolesAsClaim(user.Roles);
            return tokenManager.BuildToken();
        }
    }
}
