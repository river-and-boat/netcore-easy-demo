using System.Collections.Generic;

namespace UserService.Utils
{
    public static class UserMapper
    {
        public static Domain.User MapDataUserToDomainUser(Data.Model.User user)
        {
            if (user == null)
            {
                return null;
            }
            List<string> roles = new List<string>();
            user.Roles.ForEach(role => roles.Add(role.Role.Name));
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

        public static List<Domain.User> MapDataUsersToDomainUsers(List<Data.Model.User> users)
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
