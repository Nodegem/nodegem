using System.Threading.Tasks;

namespace ThirdParty.Data.Twilio
{
    public interface ITwilioService
    {
        Task SendSMSAsync(string accountSid, string authToken, string toPhoneNumber, string fromPhoneNumber,
            string message);
    }
}