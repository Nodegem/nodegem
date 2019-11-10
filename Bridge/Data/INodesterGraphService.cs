using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Common.Dto;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Bridge.Data
{
    public interface INodesterGraphService
    {
        Task<IEnumerable<GraphDto>> GetGraphsAsync();
        Task<MacroDto> GetMacroByIdAsync(Guid macroId);
    }
}