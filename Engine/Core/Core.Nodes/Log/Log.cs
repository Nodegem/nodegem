using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Log
{
    [DefinedNode("9E141586-C392-41C9-8188-C4FCED302137")]
    public class Log : BaseLog
    {
        public Log(ITerminalHubService logService) : base(logService)
        {
        }

        protected override async Task ExecuteLogAsync(string message, bool sendToClient)
        {
            await LogService.LogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}