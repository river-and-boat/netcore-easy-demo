using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Controller.Request
{
    public class LoginRequest
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
