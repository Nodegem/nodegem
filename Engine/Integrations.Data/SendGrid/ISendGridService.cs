using System.Threading.Tasks;

namespace Nodegem.Engine.Integrations.Data.SendGrid
{
    public interface ISendGridService
    {
        Task SendEmailAsync(string apiKey, string fromEmail, string fromName, string toEmail, string toName,
            string subject, string plainTextContent, string htmlContent);
    }
}