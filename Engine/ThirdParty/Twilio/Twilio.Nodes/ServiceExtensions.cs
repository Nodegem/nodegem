using Microsoft.Extensions.DependencyInjection;
using Nodester.ThirdParty.Twilio.Twilio.Data;

namespace Nodester.ThirdParty.Twilio.Twilio.Nodes
{
    public static class ServiceExtensions
    {
        public static void ApplyTwilioServices(this IServiceCollection services)
        {
            services.AddTransient<ITwilioService, TwilioService>();
        }
    }
}