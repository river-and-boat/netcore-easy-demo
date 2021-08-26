using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data.Model;

namespace UserService.Data.Repository
{
    public interface IUserRepository
    {
        public Task<List<User>> FindUserListAsync();

        public Task<User> FindUserByUsernameAsync(string username);

        public Task<User> CreateUserAsync(User user);

        public Task DeleteUserByUsernameAsync(string username);

        public Task UpdateUserInfoAsync(User user);

        public Task LockUserByUsernameAsync(string username);
    }
}
