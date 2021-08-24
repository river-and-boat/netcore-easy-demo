using System;
namespace NetFirstDemo.Model
{
    public class TokenManagement
    {
        public String Secret { get; set; }
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }
    }
}
