using System;

namespace Nodegem.Data.Dto
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}