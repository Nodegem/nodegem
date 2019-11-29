using System;
using System.Collections.Generic;

namespace Nodegem.Engine.Integrations.Data.Discord.Dtos
{
    public class GuildDto
    {
        public int AFKTimeout { get; set; }
        public ulong? ApplicationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int DownloadedMemberCount { get; set; }
        public IReadOnlyCollection<string> Features { get; set; }
        public bool HasAllMembers { get; set; }
        public string IconId { get; set; }
        public string IconUrl { get; set; }
        public bool IsConnected { get; set; }
        public bool IsEmbeddable { get; set; }
        public bool IsSynced { get; set; }
        public int MemberCount { get; set; }
        public string Name { get; set; }
        public ulong OwnerId { get; set; }
        public string SplashId { get; set; }
        public IReadOnlyCollection<UserDto> Users { get; set; }
    }
}