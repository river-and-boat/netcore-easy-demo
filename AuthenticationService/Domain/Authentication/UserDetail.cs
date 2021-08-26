using System;
using System.Collections.Generic;
using UserService.Exception;

namespace UserService.Domain.Authentication
{
    public class UserDetail
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Locked { get; set; }
        public List<string> Roles { get; set; }

        public void AssetUserLocked()
        {
            if (Locked)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_LOCKED_MESSAGE,
                    GlobalExceptionCode.USER_LOCKED_CODE,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }
    }
}
