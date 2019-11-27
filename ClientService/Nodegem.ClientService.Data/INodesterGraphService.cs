using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Dto;

namespace Nodegem.ClientService.Data
{
    public interface INodesterGraphService
    {
        Task<IEnumerable<GraphDto>> GetGraphsAsync();
        Task<MacroDto> GetMacroByIdAsync(Guid macroId);
    }
}