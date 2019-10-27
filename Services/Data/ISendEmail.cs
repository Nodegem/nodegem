using System.Threading.Tasks;

namespace Nodester.Services.Data
{
    public interface ISendEmail
    {
        Task<bool> SendEmailAsync(string to, string content);
    }
}