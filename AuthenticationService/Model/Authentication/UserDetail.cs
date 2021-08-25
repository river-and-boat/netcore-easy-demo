using System;
using System.Collections.Generic;

namespace AuthenticationService.Model
{
    public class UserDetail
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public bool Locked { get; set; }
        public List<String> Roles { get; set; }
    }
}
