using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Options;
using Nodester.Bridge.Extensions;
using Nodester.Common.Dto.ComponentDtos;

namespace Nodester.Bridge.Services
{
    public class NodesterUserService : NodesterAuthorizedBaseService, INodesterUserService
    {
        public NodesterUserService(HttpClient client) : base(client)
        {
        }

        public async Task<IEnumerable<ConstantDto>> GetUserConstantsAsync()
        {
            return await Client.GetAsync<IEnumerable<ConstantDto>>("account/constants");
        }
    }
}