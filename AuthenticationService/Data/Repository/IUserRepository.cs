using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public interface IUserRepository
    {
        public Task<bool> ExistUserAsync(string username);

        public Task<List<User>> FindUserListAsync();

        public Task<User> FindUserByUsernameAsync(string username);

        public Task<User> CreateUserAsync(User user);

        public Task DeleteUserByUsernameAsync(User user);

        public Task UpdateUserAsync(User user);

        public Task LockUserAsync(User user);
    }
}
