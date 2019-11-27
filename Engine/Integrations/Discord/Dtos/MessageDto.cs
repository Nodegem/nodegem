using System;
using System.Collections.Generic;
using Discord;

namespace Nodegem.Engine.Integrations.Discord.Dtos
{
    public class MessageDto
    {
        public MessageActivity Activity { get; set; }
        public MessageApplication Application { get; set; }
        public IUser Author { get; set; }
        public IMessageChannel Channel { get; set; }
        public IReadOnlyCollection<IAttachment> Attachments { get; set; }
        public bool IsTTS { get; set; }
        public bool IsPinned { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? EditedTimestamp { get; set; }
        public IReadOnlyCollection<IEmbed> Embeds { get; set; }
        public IReadOnlyCollection<ulong> MentionedRoleIds { get; set; }
        public IReadOnlyCollection<ulong> MentionedChannelIds { get; set; }
        public IReadOnlyCollection<ITag> Tags { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public MessageType Type { get; set; }
    }
}