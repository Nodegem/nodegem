using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;

namespace Nodester.Graph.Core.Nodes.Log
{
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