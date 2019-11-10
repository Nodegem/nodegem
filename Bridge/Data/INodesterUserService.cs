using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Common.Dto.ComponentDtos;

namespace Bridge.Data
{
    public interface INodesterUserService
    {
        Task<IEnumerable<ConstantDto>> GetUserConstantsAsync();
    }
}