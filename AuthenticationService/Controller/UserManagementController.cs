using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain;
using UserService.Service;

namespace UserService.Controller
{
    [ApiController]
    [Route("api")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementService userManagementService;

        public UserManagementController(UserManagementService userManagementService)
        {
            this.userManagementService = userManagementService;
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<List<User>>> GetUserList()
        {
            return Ok(await userManagementService.GetUserListAsync());
        }
    }
}
