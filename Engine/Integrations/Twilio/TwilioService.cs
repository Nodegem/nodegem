using System.Threading.Tasks;
using Nodegem.Engine.Integrations.Data.Twilio;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Nodegem.Engine.Integrations.Twilio
{
    public class TwilioService : ITwilioService
    {
        public async Task SendSMSAsync(string accountSid, string authToken, string toPhoneNumber,
            string fromPhoneNumber, string message)
        {
            TwilioClient.Init(accountSid, authToken);
            await MessageResource.CreateAsync(
                body: message,
                @from: new PhoneNumber(ConvertPhoneNumber(fromPhoneNumber)),
                to: new PhoneNumber(ConvertPhoneNumber(toPhoneNumber))
            );
            TwilioClient.Invalidate();
        }

        private string ConvertPhoneNumber(string number)
        {
            if (!number.StartsWith("+"))
            {
                if (!number.StartsWith("1"))
                {
                    return $"+1{number}";
                }

                return $"+{number}";
            }

            return number;
        }
    }
}