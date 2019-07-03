using System.Threading.Tasks;

namespace Nodester.Common.Data.Interfaces
{
    public interface ITerminalHubService
    {
        Task LogAsync(User user, string message);
        Task DebugLogAsync(User user, string message, bool isDebug);
        Task WarnLogAsync(User user, string message);
        Task ErrorLogAsync(User user, string message);
    }
}