using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Data;

namespace Nodegem.ClientService.Data
{
    public interface INodegemUserService
    {
        Task<IEnumerable<Constant>> GetUserConstantsAsync();
    }
}