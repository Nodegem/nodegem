using System.Collections.Generic;

namespace Nodegem.Engine.Integrations.Data.Discord.Dtos
{
    public class CategoryChannelDto : ChannelDto
    {
        public IReadOnlyCollection<UserDto> Users { get; set; }
    }
}