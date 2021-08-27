using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Common;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserRoleDbContext context;

        public RoleRepository(UserRoleDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> FindRoleByRoleNameAsync(RoleName roleName)
        {
            return await context
                .Role
                .AsNoTracking()
                .FirstOrDefaultAsync(role => role.Name.Equals(roleName));
        }

        public async Task<List<Role>> FindRolesByRoleNamesAsync(List<RoleName> roleNames)
        {
            return await context
                .Role
                .Where(role => roleNames.Contains(role.Name))
                .ToListAsync();
        }

        public async Task<List<Role>> FindRolesAsync()
        {
            return await context
                .Role
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistRoleAsync(RoleName roleName)
        {
            return await context
                .Role
                .AnyAsync(role => role.Name.Equals(roleName));
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            await context.AddAsync(role);
            await context.SaveChangesAsync();
            return role;
        }

        public async Task DeleteRoleByRoleName(Role role)
        {
            context.Remove(role);
            await context.SaveChangesAsync();
        }
    }
}
