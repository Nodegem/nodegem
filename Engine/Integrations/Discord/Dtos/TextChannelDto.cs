using System;
using System.Collections.Generic;

namespace Nodegem.Engine.Integrations.Discord.Dtos
{
    public class TextChannelDto
    {
        public IReadOnlyCollection<MessageDto> CachedMessages { get; set; }
        public ulong? CategoryId { get; set; }
        public bool IsNsfw { get; set; }
        public ulong Id { get; set; }
        public string Mention { get; set; }
        public string Topic { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int SlowModeInterval { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public GuildDto Guild { get; set; }
        public IReadOnlyCollection<UserDto> Users { get; set; }
    }
}