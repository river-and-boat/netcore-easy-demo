using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Data.Model;
using UserService.Exception;

namespace UserService.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRoleDbContext context;

        public UserRepository(UserRoleDbContext context)
        {
            this.context = context;
        }

        public async Task<List<User>> FindUsersAsync()
        {
            return await context
                .User
                .AsNoTracking()
                .Include(userRole => userRole.Roles)
                .ThenInclude(role => role.Role)
                .ToListAsync();
        }

        public async Task<List<User>> FindLockUsers()
        {
            return await context
                .User
                .AsNoTracking()
                .Where(user => user.Locked == true)
                .ToListAsync();
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await context
                .User
                .Include(userRole => userRole.Roles)
                .ThenInclude(role => role.Role)
                .FirstOrDefaultAsync(user => user.Name == username);
        }

        public async Task<bool> ExistUserAsync(string username)
        {
            return await context
                .User
                .AnyAsync(user => user.Name == username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await context.User.AddAsync(user);
            int result = await context.SaveChangesAsync();
            return result > 0 ? user
                : throw new GlobalException(
                    GlobalExceptionMessage.USER_CREATE_ERROR,
                    GlobalExceptionCode.USER_CREATION_ERROR_CODE,
                    GlobalStatusCode.BAD_REQUEST
                );
        }

        public async Task LockUserAsync(User user)
        {
            user.Locked = true;
            context.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task UnLockUserAsync(User user)
        {
            user.Locked = false;
            context.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserByUsernameAsync(User user)
        {
            context.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            context.User.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
