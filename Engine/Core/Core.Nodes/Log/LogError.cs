using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Log
{
    [DefinedNode("6C0B9F34-30EF-4910-B2B8-78652389391C")]
    public class LogError : BaseLog
    {
        public LogError(ITerminalHubService logService) : base(logService)
        {
        }

        protected override async Task ExecuteLogAsync(string message, bool sendToClient)
        {
            await LogService.ErrorLogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}