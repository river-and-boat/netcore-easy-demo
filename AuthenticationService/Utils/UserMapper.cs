﻿using System.Collections.Generic;
using UserService.Data.Model;

namespace UserService.Utils
{
    public static class UserMapper
    {
        public static User MapDomainUserToDataUser(Domain.User domainUser)
        {
            List<UserRole> userRoles = new();
            User user = new User()
            {
                Name = domainUser.Name,
                Password = domainUser.Password,
                Address = domainUser.Address,
                Country = domainUser.Country,
                Mobile = domainUser.Mobile,
                Email = domainUser.Email,
                Locked = domainUser.Locked,
                Roles = userRoles
            };
            domainUser.Roles.ForEach(roleName =>
            {
                Role role = new Role() { Name = roleName, Users = userRoles };
                userRoles.Add(new UserRole() { User = user, Role = role });
            });
            return user;
        }

        public static Domain.User MapDataUserToDomainUser(User user)
        {
            if (user == null)
            {
                return null;
            }
            List<string> roles = new();
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
