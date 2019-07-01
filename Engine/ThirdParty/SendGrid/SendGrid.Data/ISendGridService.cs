using System.Threading.Tasks;

namespace Nodester.ThirdParty.SendGrid.SendGrid.Data
{
    public interface ISendGridService
    {
        Task SendEmailAsync(string apiKey, string fromEmail, string fromName, string toEmail, string toName,
            string subject, string plainTextContent, string htmlContent);
    }
}