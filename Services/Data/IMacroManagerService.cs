using System;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Graph.Core.Data;

namespace Nodester.Services.Data
{
    public interface IMacroManagerService
    {
        Task<IMacroGraph> BuildMacroAsync(User user, Guid macroId);
        Task<IMacroGraph> BuildMacroAsync(User user, RunMacroDto macro);
    }
}