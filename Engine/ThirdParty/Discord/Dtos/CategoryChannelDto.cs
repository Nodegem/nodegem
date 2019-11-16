using System.Collections.Generic;

namespace Nodester.ThirdParty.Discord.Dtos
{
    public class CategoryChannelDto : ChannelDto
    {
        public IReadOnlyCollection<UserDto> Users { get; set; }
    }
}