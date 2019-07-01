using System;

namespace Bridge.Data.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }
    }
}