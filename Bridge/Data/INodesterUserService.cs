using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Data.Dto.ComponentDtos;

namespace Bridge.Data
{
    public interface INodesterUserService
    {
        Task<IEnumerable<ConstantDto>> GetUserConstantsAsync();
    }
}