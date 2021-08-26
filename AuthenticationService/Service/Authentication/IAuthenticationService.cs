using System.Threading.Tasks;
using UserService.Controller.Request;

namespace UserService.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GenerateToken(LoginRequest request);
    }
}
