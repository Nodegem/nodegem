using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Exceptions;

namespace Nodegem.Engine.Integrations.Data.Discord.Exceptions
{
    public class DiscordException : GraphRunException
    {
        public DiscordException(string message, IGraph graph) : base(message, graph)
        {
        }
    }
}