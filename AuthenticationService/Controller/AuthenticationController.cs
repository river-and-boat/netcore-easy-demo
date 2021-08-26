using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Controller.Request;
using UserService.Service.Authentication;

namespace UserService.Controller
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(
            IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult> RequestToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await authenticationService.GenerateToken(request));
        }
    }
}
