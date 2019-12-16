using System.Threading.Tasks;

namespace Nodegem.Engine.Integrations.Data.Twilio
{
    public interface ITwilioService
    {
        Task SendSMSAsync(string accountSid, string authToken, string toPhoneNumber, string fromPhoneNumber,
            string message);
    }
}