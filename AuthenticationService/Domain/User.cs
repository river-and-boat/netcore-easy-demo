using System.Collections.Generic;
using UserService.Exception;

namespace UserService.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool Locked { get; set; }
        public List<string> Roles { get; set; }

        public void AssertUserLocked()
        {
            if (Locked)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_LOCKED_MESSAGE,
                    GlobalExceptionCode.USER_LOCKED_CODE,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        public void AssertUsernamePasswordMatched(string password)
        {
            // todo decode password
            if(password != Password)
            {
                throw new GlobalException(
                    GlobalExceptionMessage.USER_PASSWORD_ERROR,
                    GlobalExceptionCode.USER_PASSWORD_WRONG_CODE,
                    GlobalStatusCode.BAD_REQUEST);
            }
        }

        public static User BuildUser(Data.Model.User user)
        {
            if (user == null)
            {
                return null;
            }
            List<string> roles = new List<string>();
            user.Roles.ForEach(role => roles.Add(role.Role.Name));
            return new User()
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
    }
}
