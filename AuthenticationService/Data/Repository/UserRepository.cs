using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRoleDbContext context;

        public UserRepository(UserRoleDbContext context)
        {
            this.context = context;
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await context
                .User
                .Include(userRole => userRole.Roles)
                .ThenInclude(role => role.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Name == username);
        }

        public Task<User> CreateUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUserByUsernameAsync(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task LockUserByUsernameAsync(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateUserInfoAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
