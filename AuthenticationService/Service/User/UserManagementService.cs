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
        private readonly IRoleRepository roleRepository;

        public UserManagementService(
            IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
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
            if (await userRepository.ExistUserAsync(request.Name))
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_HAS_EXIST,
                    GlobalExceptionCode.USER_HAS_EXIST,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            List<Data.Model.Role> roles = await roleRepository.GetRolesByRoleNames(request.Roles);
            List<Data.Model.UserRole> userRoles = new();
            Data.Model.User user = new Data.Model.User()
            {
                Name = request.Name,
                Password = request.Password,
                Address = request.Address,
                Country = request.Country,
                Email = request.Email,
                Mobile = request.Mobile,
                Locked = true,
                Roles = userRoles
            };
            roles.ForEach(role =>
            {
                userRoles.Add(new Data.Model.UserRole() { Role = role, User = user });
            });

            return UserMapper.MapDataUserToDomainUser(
                await userRepository.CreateUserAsync(user));
        }

        public async Task DeleteUser(string username)
        {
            Data.Model.User user = await userRepository.FindUserByUsernameAsync(username);
            if (user != null)
            {
                await userRepository.DeleteUserByUsernameAsync(user);
            }
            throw new GlobalException(
                GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                GlobalStatusCode.BAD_REQUEST
            );
        }
    }
}
