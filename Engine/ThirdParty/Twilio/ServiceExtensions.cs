using Microsoft.Extensions.DependencyInjection;
using ThirdParty.Data.Twilio;

namespace Nodester.ThirdParty.Twilio
{
    public static class ServiceExtensions
    {
        public static void ApplyTwilioServices(this IServiceCollection services)
        {
            services.AddTransient<ITwilioService, TwilioService>();
        }
    }
}