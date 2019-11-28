using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Dto.ComponentDtos;

namespace Nodegem.ClientService.Data
{
    public interface INodegemUserService
    {
        Task<IEnumerable<ConstantDto>> GetUserConstantsAsync();
    }
}