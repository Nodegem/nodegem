using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;

namespace Nodester.Graph.Core.Nodes.Log
{
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