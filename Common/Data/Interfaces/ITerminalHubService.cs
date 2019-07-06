using System.Threading.Tasks;

namespace Nodester.Common.Data.Interfaces
{
    public interface ITerminalHubService
    {
        Task LogAsync(User user, string message, bool sendToClient);
        Task DebugLogAsync(User user, string message, bool isDebug, bool sendToClient);
        Task WarnLogAsync(User user, string message, bool sendToClient);
        Task ErrorLogAsync(User user, string message, bool sendToClient);
    }
}