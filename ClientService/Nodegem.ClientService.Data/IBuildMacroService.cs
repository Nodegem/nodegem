using System;
using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Common.Dto;
using Nodegem.Engine.Data;

namespace Nodegem.ClientService.Data
{
    public interface IBuildMacroService
    {
        Task ExecuteMacroAsync(User user, MacroDto macro, string flowInputFieldId, bool isRunningLocally = true);

        Task<IMacroGraph> BuildMacroAsync(User user, Guid macroId);
        
        Task<IMacroGraph> BuildMacroAsync(User user, MacroDto macro);
    }
}