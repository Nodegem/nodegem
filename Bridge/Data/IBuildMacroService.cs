using System;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Engine.Data;

namespace Bridge.Data
{
    public interface IBuildMacroService
    {
        Task ExecuteMacroAsync(User user, MacroDto macro, string flowInputFieldId, bool isRunningLocally = true);

        Task<IMacroGraph> BuildMacroAsync(User user, Guid macroId);
        
        Task<IMacroGraph> BuildMacroAsync(User user, MacroDto macro);
    }
}