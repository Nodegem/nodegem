using System.Threading.Tasks;

namespace Nodegem.Engine.Integrations.Data.Mailgun
{
    public interface IMailgunService
    {
        Task SendEmailAsync(string apiKey, string requestUri, string to, string from, string subject, string message);
    }
}