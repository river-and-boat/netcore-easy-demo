using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Controller.Request
{
    public class LoginRequest
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
