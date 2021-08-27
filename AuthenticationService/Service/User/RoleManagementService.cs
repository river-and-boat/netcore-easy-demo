using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data.Model;
using UserService.Data.Repository;
using UserService.Exception;

namespace UserService.Service.User
{
    public class RoleManagementService
    {
        private readonly IRoleRepository roleRepository;

        public RoleManagementService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<Domain.Role> GetRoleByRoleNameAsync(RoleName roleName)
        {
            return UserMapper.MapDataRoleToDomainRole(
                await roleRepository.FindRoleByRoleNameAsync(roleName));
        }

        public async Task<List<Domain.Role>> GetRolesAsync()
        {
            List<Role> roles = await roleRepository.FindRolesAsync();
            return UserMapper.MapDataRolesToDomainRoles(roles);
        }

        public async Task<Domain.Role> CreateRoleAsync(RoleName roleName)
        {
            if(await roleRepository.ExistRoleAsync(roleName))
            {
                throw new GlobalException(
                    GlobalExceptionMessage.ROLE_HAS_EXIST,
                    GlobalExceptionCode.ROLE_HAS_EXIST,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            Role role = new() { Name = roleName };
            return UserMapper.MapDataRoleToDomainRole(
                await roleRepository.CreateRoleAsync(role));
        }

        public async Task DeleteRoleAsync(RoleName roleName)
        {
            Role role = await roleRepository.FindRoleByRoleNameAsync(roleName);
            await roleRepository.DeleteRoleByRoleName(role);
        }
    }
}
