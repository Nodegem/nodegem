using Microsoft.Extensions.DependencyInjection;
using Nodegem.Engine.Integrations.Data.Twilio;

namespace Nodegem.Engine.Integrations.Twilio
{
    public static class ServiceExtensions
    {
        public static void ApplyTwilioServices(this IServiceCollection services)
        {
            services.AddTransient<ITwilioService, TwilioService>();
        }
    }
}