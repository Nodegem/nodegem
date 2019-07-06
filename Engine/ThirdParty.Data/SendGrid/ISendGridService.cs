using System.Threading.Tasks;

namespace ThirdParty.Data.SendGrid
{
    public interface ISendGridService
    {
        Task SendEmailAsync(string apiKey, string fromEmail, string fromName, string toEmail, string toName,
            string subject, string plainTextContent, string htmlContent);
    }
}