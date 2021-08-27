using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserRoleDbContext context;

        public UserRoleRepository(UserRoleDbContext context)
        {
            this.context = context;
        }

        public async Task AssignRoleToUserAsync(User user, List<Role> roles)
        {
            List<UserRole> userRoles = new();
            roles.ForEach(role =>
            {
                userRoles.Add(new UserRole() { User = user, Role = role });
            });
            await context.UserRole.AddRangeAsync(userRoles);
            await context.SaveChangesAsync();
        }

        public Task DeleteRolesByUserId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> FindRolesByUserIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
