using System.Threading.Tasks;

namespace Nodegem.Common.Data.Interfaces
{
    public interface ITerminalHubService
    {
        Task LogAsync(User user, string graphId, string message, bool sendToClient);
        Task WarnLogAsync(User user, string graphId, string message, bool sendToClient);
        Task ErrorLogAsync(User user, string graphId, string message, bool sendToClient);
    }
}