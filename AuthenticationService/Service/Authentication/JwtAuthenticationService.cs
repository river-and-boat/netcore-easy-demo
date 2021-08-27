using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserService.Controller.Request;
using UserService.Exception;

namespace UserService.Service.Authentication
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly UserManagementService userManagementService;
        private readonly Domain.Token token;

        public JwtAuthenticationService(
            UserManagementService userManagementService, IConfiguration configuration)
        {
            this.userManagementService = userManagementService;
            token = configuration.GetSection("Token").Get<Domain.Token>();
        }

        public async Task<string> GenerateToken(LoginRequest request)
        {
            Domain.User user = await userManagementService.GetUserByUsernameAsync(request.Username);
            if (user != null)
            {
                user.AssertUsernamePasswordMatched(request.Password);
                user.AssertUserLocked();
                token.AddUsernameAsClaim(user.Name);
                token.AddRolesAsClaim(user.Roles);
                token.AddEmailAsClaim(user.Email);
                token.AddMobileAsClaim(user.Mobile);
                token.AddAddressAsClaim(user.Country, user.Address);
                return token.BuildToken();
            }
            throw new GlobalException(
                GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                GlobalStatusCode.BAD_REQUEST
            );
        }
    }
}
