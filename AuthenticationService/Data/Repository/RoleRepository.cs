using System;
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

        public async Task<List<Role>> GetRolesByRoleNames(List<RoleName> roleNames)
        {
            return await context
                .Role
                .Where(role => roleNames.Contains(role.Name))
                .ToListAsync();
        }
    }
}
