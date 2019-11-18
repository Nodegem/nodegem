using Microsoft.Extensions.DependencyInjection;
using ThirdParty.Data.Reddit;

namespace Nodester.ThirdParty.Reddit
{
    public static class ServiceExtensions
    {
        
        public static void ApplyRedditService(this IServiceCollection services)
        {
            services.AddScoped<IRedditService, RedditService>();
        }
        
    }
}