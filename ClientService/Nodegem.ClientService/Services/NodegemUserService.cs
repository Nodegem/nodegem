using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nodegem.ClientService.Data;
using Nodegem.ClientService.Extensions;
using Nodegem.Common.Data;

namespace Nodegem.ClientService.Services
{
    public class NodegemUserService : NodegemAuthorizedBaseService, INodegemUserService
    {
        public NodegemUserService(HttpClient client) : base(client)
        {
        }

        public async Task<IEnumerable<Constant>> GetUserConstantsAsync()
        {
            return await Client.GetAsync<IEnumerable<Constant>>("account/constants");
        }
    }
}