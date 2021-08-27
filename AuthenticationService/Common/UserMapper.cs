using System.Collections.Generic;
using UserService.Data.Model;

namespace UserService.Common
{
    public static class UserMapper
    {
        public static Domain.User MapDataUserToDomainUser(User user)
        {
            if (user == null)
            {
                return null;
            }
            List<string> roles = new();
            user.Roles.ForEach(role => roles.Add(role.Role.Name.ToString()));
            return new Domain.User()
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Address = user.Address,
                Country = user.Country,
                Mobile = user.Mobile,
                Email = user.Email,
                Locked = user.Locked,
                Roles = roles
            };
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
    }
}
