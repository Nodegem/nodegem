using Microsoft.Extensions.DependencyInjection;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Nodes;
using Nodester.Services.Data;
using Nodester.Services.Data.Repositories;
using Nodester.Services.Repositories;
using Nodester.ThirdParty.SendGrid.SendGrid.Nodes;
using Nodester.ThirdParty.Twilio.Twilio.Nodes;

namespace Nodester.Services
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Nodester.Data.Models.Graph>, Repository<Nodester.Data.Models.Graph>>();
            services.AddTransient<IMacroRepository, MacroRepository>();
            services.AddTransient<IGraphRepository, GraphRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();

            services.RegisterMySelf();
            services.ApplyNodeServices();
            services.ApplyTwilioServices();
            services.ApplySendGridServices();
        }

        public static void AddServicesForBridge(this IServiceCollection services)
        {
            services.RegisterMySelf();
            services.ApplyNodeServices();
            services.ApplyTwilioServices();
            services.ApplySendGridServices();
        }
    }
}