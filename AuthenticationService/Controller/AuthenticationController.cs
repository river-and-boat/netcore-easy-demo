using System;
using AuthenticationService.Controller.Request;
using AuthenticationService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetFirstDemo.Service.authentication;

namespace AuthenticationService.Controller
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly TokenConfig tokenConfig;

        public AuthenticationController(
            IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.tokenConfig = configuration.GetSection("Token").Get<TokenConfig>();
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
