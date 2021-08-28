using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Common;
using UserService.Controller.Request;
using UserService.Domain;
using UserService.Exception;
using UserService.Service.User;

namespace UserService.Controller
{
    [ApiController]
    [Route("api/roles")]
    public class RoleManagementController : ControllerBase
    {
        private readonly RoleManagementService roleManagementService;
        private readonly ILogger<RoleManagementController> logger;

        public RoleManagementController(
            RoleManagementService roleManagementService, ILogger<RoleManagementController> logger)
        {
            this.roleManagementService = roleManagementService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{rolename}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Role>> GetRole(string rolename)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), rolename.ToUpper());
                logger.LogInformation("select role by role name: {0}", name);
                return Ok(await roleManagementService.GetRoleByRoleNameAsync(name));
            }
            catch (System.Exception ex)
            {
                logger.LogError("error occuered when get role: {0}", ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            return Ok(await roleManagementService.GetRolesAsync());
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> CreateRole([FromBody] RoleChangeRequest request)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), request.Name.ToUpper());
                Role role = await roleManagementService.CreateRoleAsync(name);
                logger.LogInformation("create role: {0}", role);
                return CreatedAtAction(nameof(GetRole), new { rolename = role.Name }, role);
            }
            catch (System.Exception ex)
            {
                logger.LogError("error occured when create role: {0}", ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        [HttpDelete]
        [Route("{rolename}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> DeleteRole(string rolename)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), rolename.ToUpper());
                await roleManagementService.DeleteRoleAsync(name);
                logger.LogInformation("delete role: {0}", name);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                logger.LogError("error occured when delete role: {0}", ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }
    }
}
