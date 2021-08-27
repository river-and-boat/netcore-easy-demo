using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Controller.Request;
using UserService.Data.Repository;
using UserService.Domain;
using UserService.Exception;
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

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return UserMapper.MapDataUserToDomainUser(
                await userRepository.FindUserByUsernameAsync(username));
        }

        public async Task<List<User>> GetUserListAsync()
        {
            return UserMapper.MapDataUsersToDomainUsers(
                await userRepository.FindUserListAsync());
        }

        public async Task LockUserAsync(string username)
        {
            Data.Model.User user = await userRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                await userRepository.LockUserAsync(user);
            }
            throw new GlobalException(
                GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                GlobalStatusCode.BAD_REQUEST
            );
        }

        public async Task<User> CreateUser(CreateUserRequest request)
        {
            User user = new User()
            {
                Name = request.Name,
                Password = request.Password,
                Address = request.Address,
                Country = request.Country,
                Email = request.Email,
                Mobile = request.Mobile,
                Locked = true,
                Roles = request.Roles
            };

            return UserMapper.MapDataUserToDomainUser(
                await userRepository.CreateUserAsync(
                    UserMapper.MapDomainUserToDataUser(user)));
        }
    }
}
