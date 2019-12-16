using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Core.Nodes.Log
{
    [DefinedNode("F00FC4B5-9D95-429D-9FFD-D3B0C91AD1D8")]
    public class LogWarn : BaseLog
    {
        public LogWarn(ITerminalHubService logService) : base(logService)
        {
        }

        protected override async Task ExecuteLogAsync(string message, bool sendToClient)
        {
            await LogService.WarnLogAsync(Graph.User, Graph.Id.ToString(), message, sendToClient);
        }
    }
}