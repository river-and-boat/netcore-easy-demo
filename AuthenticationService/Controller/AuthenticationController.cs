using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserService.Controller.Request;
using UserService.Domain.Authentication;
using UserService.Service.Authentication;

namespace UserService.Controller
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly TokenManager tokenConfig;

        public AuthenticationController(
            IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.tokenConfig = configuration.GetSection("Token").Get<TokenManager>();
        }

        [HttpPost]
        [Route("token")]
        public ActionResult RequestToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid || !authenticationService.IsAuthenticated(request))
            {
                return BadRequest();
            }
            return Ok(authenticationService.GenerateToken(request.Username, tokenConfig));
        }
    }
}
