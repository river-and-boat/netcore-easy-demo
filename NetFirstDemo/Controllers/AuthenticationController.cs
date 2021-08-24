using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetFirstDemo.Model;
using NetFirstDemo.Service.authentication;

namespace NetFirstDemo.Controllers
{
    [ApiController]
    [Route("api")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly TokenManagement tokenManagement;

        public AuthenticationController(
            IAuthenticationService authenticationService, IConfiguration configuration)
        {
            this.authenticationService = authenticationService;
            this.tokenManagement = configuration.GetSection("tokenManagement").Get<TokenManagement>();
        }

        [HttpPost]
        [Route("token")]
        public ActionResult RequestToken([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid || !authenticationService.IsAuthenticated(request))
            {
                return BadRequest();
            }
            return Ok(authenticationService.GenerateToken(request.Username, tokenManagement));
        }
    }
}
