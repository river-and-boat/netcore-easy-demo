using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public RoleManagementController(RoleManagementService roleManagementService)
        {
            this.roleManagementService = roleManagementService;
        }

        [HttpGet]
        [Route("{rolename}")]
        public async Task<ActionResult<Role>> GetRole(string rolename)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), rolename.ToUpper());
                return Ok(await roleManagementService.GetRoleByRoleNameAsync(name));
            }
            catch (System.Exception ex)
            {
                // todo Log
                Console.WriteLine("exception occured: " + ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            return Ok(await roleManagementService.GetRolesAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody] RoleChangeRequest request)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), request.Name.ToUpper());
                Role role = await roleManagementService.CreateRoleAsync(name);
                return CreatedAtAction(nameof(GetRole), new { rolename = role.Name }, role);
            }
            catch (System.Exception ex)
            {
                // todo Log
                Console.WriteLine("exception occured: " + ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        [HttpDelete]
        [Route("{rolename}")]
        public async Task<ActionResult> DeleteRole(string rolename)
        {
            try
            {
                RoleName name = (RoleName)Enum.Parse(typeof(RoleName), rolename.ToUpper());
                await roleManagementService.DeleteRoleAsync(name);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                // todo Log
                Console.WriteLine("exception occured: " + ex.Message);
                throw new GlobalException(
                    GlobalExceptionMessage.INVALID_ROLE_NAME,
                    GlobalExceptionCode.INVALID_ROLE_NAME,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }
    }
}
