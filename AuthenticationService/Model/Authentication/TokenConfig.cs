using System;
namespace AuthenticationService.Model
{
    public class TokenConfig
    {
        public String Secret { get; set; }
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }
    }
}
