using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private static readonly string FILE_PATH = "//Images//";

        private readonly UserManagementService userManagementService;
        private readonly ILogger<UserManagementController> logger;
        private readonly IWebHostEnvironment environment;

        public UserManagementController(
            UserManagementService userManagementService,
            ILogger<UserManagementController> logger,
            IWebHostEnvironment environment)
        {
            this.userManagementService = userManagementService;
            this.logger = logger;
            this.environment = environment;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await userManagementService.GetUsersAsync());
        }

        [HttpGet]
        [Route("lock")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<List<User>>> GetLockedUsers()
        {
            return Ok(await userManagementService.GetLockedUsersAsync());
        }

        [HttpGet]
        [Route("self")]
        [Authorize(Roles = "ADMIN, USER, CUSTOMER")]
        public async Task<ActionResult<User>> GetSelfInfo()
        {
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            return await GetUserByUsername(loginUsername);
        }

        [HttpGet]
        [Route("{username}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            logger.LogInformation("select user by name: {0}", username);
            return Ok(await userManagementService.GetUserByUsernameAsync(username));
        }

        [HttpPatch]
        [Route("{username}/lock")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> LockUser(string username)
        {
            logger.LogInformation("lock the user: {0}", username);
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            if (!username.Equals(loginUsername))
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> UnLockUser(string username)
        {
            logger.LogInformation("unlock the user: {0}", username);
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            if (!username.Equals(loginUsername))
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
        [AllowAnonymous]
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            logger.LogInformation("delete the user: {0}", username);
            await userManagementService.DeleteUser(username);
            return NoContent();
        }

        [HttpPost]
        [Route("{username}/roles")]
        [Authorize(Roles = "ADMIN")]
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

        [HttpPost]
        [Route("avatar")]
        [Authorize(Roles = "ADMIN, USER, CUSTOMER")]
        public async Task<ActionResult> UploadAvatar(IFormFile avatar)
        {
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            string filepath = environment.ContentRootPath + FILE_PATH;
            string extension = Path.GetExtension(avatar.FileName).ToLower();
            if (avatar.Length > 0 && (extension == ".jpg" || extension == ".jpeg" || extension == ".png"))
            {
                await userManagementService.UploadAvatarAsync(filepath, avatar.FileName, loginUsername, avatar);
                return NoContent();
            }
            throw new GlobalException(
                GlobalExceptionMessage.INVALID_AVATAR_FORMAT,
                GlobalExceptionCode.INVALID_AVATAR_FORMAT,
                GlobalStatusCode.BAD_REQUEST
            );
        }

        [HttpGet]
        [Route("avatar")]
        [Authorize(Roles = "ADMIN, USER, CUSTOMER")]
        public async Task<FileContentResult> GetAvatar()
        {
            string loginUsername = User.FindFirstValue(ClaimTypes.Name);
            Tuple<byte[], string> resource = await userManagementService.GetAvatar(loginUsername,
                environment.ContentRootPath + FILE_PATH);
            return File(resource.Item1, "image" + resource.Item2.Replace(".", "/"));
        }
    }
}
