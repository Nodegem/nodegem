using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nodegem.ClientService.Data;
using Nodegem.ClientService.Extensions;
using Nodegem.Common.Dto.ComponentDtos;

namespace Nodegem.ClientService.Services
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