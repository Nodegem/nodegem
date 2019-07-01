using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Graph.Core.Data.Exceptions;
using Nodester.Services.Data;
using Nodester.Services.Data.Hubs;

namespace Nodester.Hubs
{
    [Authorize]
    public class GraphHub : Hub
    {
        private readonly IGraphManagerService _graphManagerService;
        private readonly IMacroManagerService _macroManagerService;
        private readonly ITerminalHubService _terminal;
        private readonly IUserService _userService;

        public GraphHub(IGraphManagerService graphManagerService, IMacroManagerService macroManagerService, ITerminalHubService terminal, IUserService userService)
        {
            _macroManagerService = macroManagerService;
            _graphManagerService = graphManagerService;
            _terminal = terminal;
            _userService = userService;
        }

        public async Task RunGraph(RunGraphDto data)
        {
            var userId = Context.User.GetUserId();
            User user = null;
            
            try
            {
                var userConstants = await _userService.GetConstantsAsync(userId); 
                user = new User
                {
                    ConnectionId = Context.ConnectionId,
                    Email = Context.User.GetEmail(),
                    Id = userId.ToString(),
                    Username = Context.User.GetUsername(),
                    Constants = userConstants
                        .ToDictionary(k => k.Key, c => c.Adapt<Constant>())
                };
                
                await _terminal.SendDebugLogAsync(user, "Building Graph...", data.IsDebugModeEnabled);
                var graph = await _graphManagerService.BuildGraph(user, data);
                await _terminal.SendDebugLogAsync(user, "Running Graph...", data.IsDebugModeEnabled);
                graph.Run();
            }
            catch (GraphException ex)
            {
                await _terminal.SendErrorLogAsync(user, ex.Message);
            }
            catch (Exception ex)
            {
                await _terminal.SendErrorLogAsync(user, "Error occurred while running graph.");
                Console.WriteLine(ex.Message);
            }
        }
        
        public async Task RunMacro(RunMacroDto macroData, string flowInputFieldKey)
        {
            var user = new User
            {
                ConnectionId = Context.ConnectionId,
                Email = Context.User.GetEmail(),
                Id = Context.User.GetUserId().ToString(),
                Username = Context.User.GetUsername()
            };
            
            try
            {
                await _terminal.SendDebugLogAsync(user, "Building macro...", macroData.IsDebugModeEnabled);
                var macro = await _macroManagerService.BuildMacroAsync(user, macroData);
                await _terminal.SendDebugLogAsync(user, "Running macro...", macroData.IsDebugModeEnabled);
                macro.Run(flowInputFieldKey);
            }
            catch (GraphException ex)
            {
                await _terminal.SendErrorLogAsync(user, ex.Message);
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                await _terminal.SendErrorLogAsync(user, $"Something went wrong: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}