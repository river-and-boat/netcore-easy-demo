using System.Collections.Generic;
using UserService.Data.Model;

namespace UserService.Common
{
    public static class UserMapper
    {
        public static Domain.User MapDataUserToDomainUser(User user)
        {
            if (user != null)
            {
                List<string> roles = new();
                user.Roles.ForEach(role => roles.Add(role.Role.Name.ToString()));
                return new Domain.User()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = AesAlgorithms.DecryptAes(user.Password),
                    Address = user.Address,
                    Country = user.Country,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    Locked = user.Locked,
                    Roles = roles
                };
            }
            return null;
        }

        public static List<Domain.User> MapDataUsersToDomainUsers(List<User> users)
        {
            List<Domain.User> result = new();
            users.ForEach(user =>
            {
                result.Add(MapDataUserToDomainUser(user));
            });
            return result;
        }

        public static Domain.Role MapDataRoleToDomainRole(Role role)
        {
            if (role != null)
            {
                return new Domain.Role()
                {
                    Name = role.Name.ToString()
                };
            }
            return null;
        }

        public static List<Domain.Role> MapDataRolesToDomainRoles(List<Role> roles)
        {
            List<Domain.Role> result = new();
            roles.ForEach(role =>
            {
                result.Add(MapDataRoleToDomainRole(role));
            });
            return result;
        }
    }
}
