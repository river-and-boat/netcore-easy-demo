using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Controller.Request;
using UserService.Data.Repository;
using UserService.Exception;
using UserService.Common;
using UserService.Data.Model;

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

        public async Task<Domain.User> GetUserByUsernameAsync(string username)
        {
            return UserMapper.MapDataUserToDomainUser(
                await userRepository.FindUserByUsernameAsync(username));
        }

        public async Task<List<Domain.User>> GetUserListAsync()
        {
            return UserMapper.MapDataUsersToDomainUsers(
                await userRepository.FindUserListAsync());
        }

        public async Task LockUserAsync(string username)
        {
            Data.Model.User user = await userRepository.FindUserByUsernameAsync(username);
            if (user == null)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                    GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            await userRepository.LockUserAsync(user);
        }

        public async Task<Domain.User> CreateUser(CreateUserRequest request)
        {
            if (await userRepository.ExistUserAsync(request.Name))
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_HAS_EXIST,
                    GlobalExceptionCode.USER_HAS_EXIST,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            List<Role> roles = await roleRepository.FindRolesByRoleNamesAsync(request.Roles);
            List<UserRole> userRoles = new();
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
                userRoles.Add(new UserRole() { Role = role, User = user });
            });
            return UserMapper.MapDataUserToDomainUser(
                await userRepository.CreateUserAsync(user));
        }

        public async Task DeleteUser(string username)
        {
            Data.Model.User user = await userRepository.FindUserByUsernameAsync(username);
            if (user == null)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                    GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            await userRepository.DeleteUserByUsernameAsync(user);
        }
    }
}
