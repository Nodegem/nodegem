using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;

namespace Nodester.ThirdParty.Discord.Exceptions
{
    public class DiscordException : GraphException
    {
        public DiscordException(string message, IGraph graph) : base(message, graph)
        {
        }
    }
}