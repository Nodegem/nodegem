using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;

namespace Nodester.Graph.Core.Nodes.Logging
{
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