using System.Collections.Generic;
using UserService.Controller.Request;
using UserService.Domain.Authentication;

namespace UserService.Service.Authentication
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly Dictionary<string, UserDetail> USERS = new Dictionary<string, UserDetail>()
        {
            {
                "river-and-boat",
                new UserDetail()
                {
                    Locked = false,
                    Username = "river-and-boat",
                    Password = "wait for secret",
                    Roles = new List<string>
                    {
                        "admin", "user", "customer"
                    }
                }
            },
            {
                "vera_zhang",
                new UserDetail()
                {
                    Locked = true,
                    Username = "vera_zhang",
                    Password = "wait for secret",
                    Roles = new List<string>
                    {
                        "user", "customer"
                    }
                }
            }
        };

        public UserDetail GetUser(string username)
        {
            // todo get user from database
            return USERS[username];
        }

        public bool IsAuthenticated(LoginRequest request)
        {
            // todo: directly return true, get username and password from datastore
            return true;
        }
    }
}
