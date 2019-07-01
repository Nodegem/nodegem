using System.Threading.Tasks;
using Nodester.Common.Data;

namespace Nodester.Services.Data.Hubs
{
    public interface ITerminalHubService
    {
        Task SendLogAsync(User user, string message);
        Task SendDebugLogAsync(User user, string message, bool isDebug);
        Task SendErrorLogAsync(User user, string message);
        Task SendWarnLogAsync(User user, string message);
    }
}