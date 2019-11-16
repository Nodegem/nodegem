using System;
using Discord;

namespace Nodester.ThirdParty.Discord.Dtos
{
    public class UserDto
    {
        public IActivity Activity { get; set; }
        public string AvatarId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Discriminator { get; set; }
        public bool IsBot { get; set; }
        public bool IsWebhook { get; set; }
        public string Mention { get; set; }
        public UserStatus Status { get; set; }
        public string Username { get; set; }
    }
}