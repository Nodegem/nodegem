using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Options;
using Nodester.Bridge.Extensions;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Bridge.Services
{
    public class NodesterGraphService : NodesterAuthorizedBaseService, INodesterGraphService
    {
        
        public NodesterGraphService(HttpClient client) : base(client)
        {
        }

        public async Task<IEnumerable<GraphDto>> GetGraphsAsync()
        {
            return await Client.GetAsync<IEnumerable<GraphDto>>("graph/all");
        }
        
        public async Task<MacroDto> GetMacroByIdAsync(Guid macroId)
        {
            return await Client.GetAsync<MacroDto>($"macro/{macroId}");
        }
    }
}