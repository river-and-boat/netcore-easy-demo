using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Common;
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
        private readonly ILogger<UserManagementController> logger;

        public UserManagementController(
            UserManagementService userManagementService, ILogger<UserManagementController> logger)
        {
            this.userManagementService = userManagementService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await userManagementService.GetUsersAsync());
        }

        [HttpGet]
        [Route("lock")]
        public async Task<ActionResult<List<User>>> GetLockedUsers()
        {
            return Ok(await userManagementService.GetLockedUsersAsync());
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            logger.LogInformation("select user by name: {0}", username);
            return Ok(await userManagementService.GetUserByUsernameAsync(username));
        }

        [HttpPatch]
        [Route("{username}/lock")]
        public async Task<ActionResult> LockUser(string username)
        {
            logger.LogInformation("lock the user: {0}", username);
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

        [HttpPatch]
        [Route("{username}/unlock")]
        public async Task<ActionResult> UnLockUser(string username)
        {
            logger.LogInformation("unlock the user: {0}", username);
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            if (loginUsername != null && !username.Equals(loginUsername))
            {
                await userManagementService.UnLockUserAsync(username);
                return NoContent();
            }
            throw new GlobalException(
                GlobalExceptionMessage.USER_UNLOCK_SELF_ERROR,
                GlobalExceptionCode.USER_UNLOCK_SELF_CODE,
                GlobalStatusCode.BAD_REQUEST
            );
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("create user failed: {0}", ModelState.Values);
                return BadRequest();
            }
            logger.LogInformation("create the user: {0}", request.Name);
            User user = await userManagementService.CreateUser(request);
            return CreatedAtAction(nameof(GetUserByUsername), new { username = user.Name }, user);
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            logger.LogInformation("delete the user: {0}", username);
            await userManagementService.DeleteUser(username);
            return NoContent();
        }

        [HttpPost]
        [Route("{username}/roles")]
        public async Task<ActionResult> AssignRoles(
            string username, [FromBody] List<RoleChangeRequest> requests)
        {
            List<RoleName> roleNames = new();
            requests.ForEach(request =>
            {
                try
                {
                    RoleName name = (RoleName)Enum.Parse(typeof(RoleName), request.Name.ToUpper());
                    roleNames.Add(name);
                }
                catch (System.Exception ex)
                {
                    logger.LogError("The given role is not exist: {0}", ex.Message);
                }
            });
            logger.LogInformation("assign roles: {0} to the user: {1}", roleNames, username);
            await userManagementService.AssignRoles(username, roleNames);
            return NoContent();
        }
    }
}
