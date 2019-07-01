using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nodester.Common.Data.Interfaces;
using Nodester.Data.Dto;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Data.Dto.UserDtos;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;
using Nodester.Graph.Core;
using Nodester.Graph.Core.Nodes;
using Nodester.Services.Data;
using Nodester.Services.Data.Hubs;
using Nodester.Services.Data.Mappers;
using Nodester.Services.Data.Repositories;
using Nodester.Services.Hubs;
using Nodester.Services.Mappers;
using Nodester.Services.Repositories;
using Nodester.ThirdParty.SendGrid.SendGrid.Nodes;
using Nodester.ThirdParty.Twilio.Twilio.Nodes;

namespace Nodester.Services
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddTransient<IRepository<Nodester.Data.Models.Graph>, Repository<Nodester.Data.Models.Graph>>();
            collection.AddTransient<IMacroRepository, MacroRepository>();
            collection.AddTransient<IGraphRepository, GraphRepository>();
            collection.AddTransient<ITokenRepository, TokenRepository>();

            collection.AddTransient<IUserService, UserService>();
            collection.AddTransient<ITokenService, TokenService>();

            collection.AddTransient<IMapper<ApplicationUser, RegisterDto>, Mapper<ApplicationUser, RegisterDto>>();
            collection.AddTransient<IMapper<ApplicationUser, UserDto>, Mapper<ApplicationUser, UserDto>>();
            collection.AddTransient<IMapper<AccessToken, TokenDto>, Mapper<AccessToken, TokenDto>>();

            collection
                .AddTransient<IMapper<Nodester.Data.Models.Graph, CreateGraphDto>,
                    Mapper<Nodester.Data.Models.Graph, CreateGraphDto>>();
            collection
                .AddTransient<IMapper<Nodester.Data.Models.Graph, GraphDto>,
                    Mapper<Nodester.Data.Models.Graph, GraphDto>>();

            collection.AddTransient<IMapper<Macro, CreateMacroDto>, Mapper<Macro, CreateMacroDto>>();
            collection.AddTransient<IMapper<Macro, MacroDto>, Mapper<Macro, MacroDto>>();
            collection.AddTransient<IMapper<MacroDto, RunMacroDto>, Mapper<MacroDto, RunMacroDto>>();

            collection.AddTransient<ILogService, LogService>();

            collection.AddTransient<ITerminalHubService, TerminalHubService>();

            collection.AddScoped<IGraphManagerService, GraphManagerService>();
            collection.AddScoped<IMacroManagerService, MacroManagerService>();

            collection.RegisterMySelf();
            collection.ApplyNodeServices();
            collection.ApplyTwilioServices();
            collection.ApplySendGridServices();
        }
    }
}