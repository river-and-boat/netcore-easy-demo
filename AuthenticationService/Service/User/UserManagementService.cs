using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data.Repository;
using UserService.Domain;
using UserService.Utils;

namespace UserService.Service
{
    public class UserManagementService
    {
        private readonly IUserRepository userRepository;

        public UserManagementService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<List<User>> GetUserListAsync()
        {
            return UserMapper.MapDataUsersToDomainUsers(
                await userRepository.FindUserListAsync());
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return UserMapper.MapDataUserToDomainUser(
                await userRepository.FindUserByUsernameAsync(username));
        }

        public async Task<User> CreateUser()
        {
            return null;
        }
    }
}
