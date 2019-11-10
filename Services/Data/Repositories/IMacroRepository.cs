using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Common.Dto;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Services.Data.Repositories
{
    public interface IMacroRepository
    {
        IEnumerable<MacroDto> GetMacrosAssignedToUser(Guid userId);
        Task<MacroDto> GetById(Guid id);
        MacroDto CreateNewMacro(CreateMacroDto newMacro);
        MacroDto UpdateMacro(Guid id, MacroDto macro);
        void DeleteMacro(Guid macroId);
    }
}