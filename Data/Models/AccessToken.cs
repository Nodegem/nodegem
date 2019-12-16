using System;
using System.ComponentModel.DataAnnotations.Schema;
using Mapster;

namespace Nodegem.Data.Models
{
    public class AccessToken : BaseEntity
    {
        [AdaptMember("AccessToken")] public string Token { get; set; }

        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")] public ApplicationUser User { get; set; }
    }
}