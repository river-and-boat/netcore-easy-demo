using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data.Repository;
using UserService.Exception;

namespace UserService.Service.User
{
    public class UserRoleRelationshipService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;

        public UserRoleRelationshipService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
        }

        public async Task AssignRoleToUser(string username, List<RoleName> roleNames)
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
            user.Roles.ForEach(role =>
            {
                if (roleNames.Contains(role.Role.Name))
                {
                    roleNames.Remove(role.Role.Name);
                }
            });
            var roles = await roleRepository.FindRolesByRoleNamesAsync(roleNames);
            await userRoleRepository.AssignRoleToUserAsync(user, roles);
        }

        public async Task UpdateUserRole(string username, List<RoleName> roleNames)
        {

        }
    }
}
