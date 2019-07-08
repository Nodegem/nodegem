using Microsoft.Extensions.DependencyInjection;
using ThirdParty.Data.Discord;

namespace Nodester.ThirdParty.Discord
{
    public static class ServiceExtensions
    {
        
        public static void ApplyDiscordServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscordService, DiscordService>();
        }
        
    }
}