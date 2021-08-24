using System;
using System.ComponentModel.DataAnnotations;

namespace NetFirstDemo.Model
{
    public class LoginRequestDTO
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
