using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public interface IRoleRepository
    {
        public Task<Role> FindRoleByRoleNameAsync(RoleName roleName);
        public Task<List<Role>> FindRolesByRoleNamesAsync(List<RoleName> roleNames);
        public Task<List<Role>> FindRolesAsync();
        public Task DeleteRoleByRoleName(Role role);
        public Task<Role> CreateRoleAsync(Role role);
        public Task<bool> ExistRoleAsync(RoleName roleName);
    }
}
