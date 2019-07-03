using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Nodester.Data.Contexts;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Data.Models;
using Nodester.Services.Data.Repositories;

namespace Nodester.Services.Repositories
{
    public class MacroRepository : Repository<Macro>, IMacroRepository
    {
        public MacroRepository(NodesterDBContext context) : base(context)
        {
        }

        public IEnumerable<MacroDto> GetAllMacros(Guid userId)
        {
            return GetAll(x => x.UserId == userId).Select(x => x.Adapt<MacroDto>());
        }

        public async Task<MacroDto> GetById(Guid id)
        {
            var macro = await GetAsync(id);
            return macro.Adapt<MacroDto>();
        }

        public MacroDto CreateNewMacro(CreateMacroDto newMacro)
        {
            var macro = newMacro.Adapt<Macro>();
            Create(macro);
            return macro.Adapt<MacroDto>();
        }

        public MacroDto UpdateMacro(Guid macroId, MacroDto macro)
        {
            var updatedMacro = macro.Adapt<Macro>();
            Update(macroId, updatedMacro);
            return updatedMacro.Adapt<MacroDto>();
        }

        public void DeleteMacro(Guid macroId)
        {
            Delete(macroId);
        }
    }
}