using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;

namespace Nodester.ThirdParty.Discord.Exceptions
{
    public class DiscordException : GraphRunException
    {
        public DiscordException(string message, IGraph graph) : base(message, graph)
        {
        }
    }
}