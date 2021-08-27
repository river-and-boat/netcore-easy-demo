using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Controller.Request;
using UserService.Domain;
using UserService.Exception;
using UserService.Service;

namespace UserService.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementService userManagementService;

        public UserManagementController(UserManagementService userManagementService)
        {
            this.userManagementService = userManagementService;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            return Ok(await userManagementService.GetUserByUsernameAsync(username));
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUserList()
        {
            return Ok(await userManagementService.GetUserListAsync());
        }

        [HttpPatch]
        [Route("{username}")]
        public async Task<ActionResult> LockUser(string username)
        {
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            if (loginUsername != null && !username.Equals(loginUsername))
            {
                await userManagementService.LockUserAsync(username);
                return NoContent();
            }
            throw new GlobalException(
                GlobalExceptionMessage.USER_LOCK_SELF_ERROR,
                GlobalExceptionCode.USER_LOCKED_CODE,
                GlobalStatusCode.BAD_REQUEST
            );
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = await userManagementService.CreateUser(request);
            return CreatedAtAction(nameof(GetUserByUsername), new { username = user.Name }, user);
        }
    }
}
