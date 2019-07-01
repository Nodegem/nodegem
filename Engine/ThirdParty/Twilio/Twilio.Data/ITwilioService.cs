using System.Threading.Tasks;

namespace Nodester.ThirdParty.Twilio.Twilio.Data
{
    public interface ITwilioService
    {
        Task SendSMSAsync(string accountSid, string authToken, string toPhoneNumber, string fromPhoneNumber,
            string message);
    }
}