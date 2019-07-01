using System.Threading.Tasks;

namespace Nodester.Common.Data.Interfaces
{
    public interface ILogService
    {
        Task SendLogAsync(User user, string message);
        Task SendWarnLogAsync(User user, string message);
        Task SendErrorLogAsync(User user, string message);
    }
}