using System.Collections.Generic;

namespace Nodegem.Engine.Integrations.Discord.Dtos
{
    public class CategoryChannelDto : ChannelDto
    {
        public IReadOnlyCollection<UserDto> Users { get; set; }
    }
}