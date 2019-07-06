using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using ThirdParty.Data.SendGrid;

namespace Nodester.ThirdParty.SendGrid
{
    public class SendGridService : ISendGridService
    {
        public async Task SendEmailAsync(string apiKey, string fromEmail, string fromName, string toEmail,
            string toName,
            string subject,
            string plainTextContent, string htmlContent)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };
            msg.AddTo(new EmailAddress(toEmail, toName));
            await new SendGridClient(apiKey).SendEmailAsync(msg);
        }
    }
}