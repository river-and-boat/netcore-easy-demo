using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UserService.Common;

namespace UserService.Controller.Request
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }

        public List<RoleName> Roles { get; set; }

        public CreateUserRequest()
        {
            Roles = new();
        }
    }
}
