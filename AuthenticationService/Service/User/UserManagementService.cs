using System.Threading.Tasks;
using UserService.Data.Repository;
using UserService.Domain;

namespace UserService.Service
{
    public class UserManagementService
    {
        private readonly IUserRepository userRepository;

        public UserManagementService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return User.BuildUser
            (
                await userRepository.FindUserByUsernameAsync(username)
            );
        }
    }
}
