using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Controller.Request;
using UserService.Data.Repository;
using UserService.Exception;
using UserService.Common;
using UserService.Data.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

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

        public async Task<List<Domain.User>> GetUsersAsync()
        {
            return UserMapper.MapDataUsersToDomainUsers(
                await userRepository.FindUsersAsync());
        }

        public async Task<List<Domain.User>> GetLockedUsersAsync()
        {
            return UserMapper.MapDataUsersToDomainUsers(
                await userRepository.FindLockUsers());
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

        public async Task UnLockUserAsync(string username)
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
            await userRepository.UnLockUserAsync(user);
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
            Role role = await roleRepository.FindRoleByRoleNameAsync(RoleName.CUSTOMER);
            List<UserRole> userRoles = new();
            Data.Model.User user = new Data.Model.User()
            {
                Name = request.Name,
                Password = AesAlgorithms.EncryptAes(request.Password),
                Address = request.Address,
                Country = request.Country,
                Email = request.Email,
                Mobile = request.Mobile,
                Locked = true,
                Roles = userRoles
            };
            if (role != null)
            {
                userRoles.Add(new UserRole() { Role = role, User = user });
            }
            else
            {
                userRoles.Add(new UserRole() { Role = new Role() { Name = RoleName.CUSTOMER }, User = user });
            }
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

        public async Task AssignRoles(string username, List<RoleName> roleNames)
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
            List<Role> roles = await roleRepository.FindRolesByRoleNamesAsync(roleNames);
            List<UserRole> userRoles = new();
            roles.ForEach(role =>
            {
                userRoles.Add(new UserRole() { User = user, Role = role });
            });
            user.Roles = userRoles;
            await userRepository.UpdateUserAsync(user);
        }

        public async Task UploadAvatarAsync(
            string rootPath, string filename, string username, IFormFile avatar)
        {
            string relativePath = Guid.NewGuid() + "_"
                + filename.Substring(filename.LastIndexOf("//") + 1);
            string fullPath = rootPath + relativePath;
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
                await stream.FlushAsync();
            }
            Data.Model.User user = await userRepository.FindUserByUsernameAsync(username);
            if (user == null)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_NAME_NOT_EXIST,
                    GlobalExceptionCode.USER_NAME_NOT_EXIST_CODE,
                    GlobalStatusCode.BAD_REQUEST
                );
            }
            user.AvatarUrl = relativePath;
            await userRepository.UpdateUserAsync(user);
        }

        public async Task<Tuple<byte[], string>> GetAvatar(string username, string rootPath)
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
            string fullPath = rootPath + "//" + user.AvatarUrl;
            FileInfo fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists)
            {
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    byte[] buffer = new byte[fileInfo.Length];
                    await fileStream.ReadAsync(buffer, 0, Convert.ToInt32(fileInfo.Length));
                    return new Tuple<byte[], string>(buffer, fileInfo.Extension);
                }
            }
            throw new GlobalException(
                GlobalExceptionMessage.AVATAR_NOT_EXIST,
                GlobalExceptionCode.AVATAR_NOT_EXIST,
                GlobalStatusCode.BAD_REQUEST
            );
        }
    }
}
