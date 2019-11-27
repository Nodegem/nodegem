using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Nodegem.Common.Dto;
using Nodegem.Data.Contexts;
using Nodegem.Data.Dto.MacroDtos;
using Nodegem.Data.Models;
using Nodegem.Services.Data.Repositories;

namespace Nodegem.Services.Repositories
{
    public class MacroRepository : Repository<Macro>, IMacroRepository
    {
        public MacroRepository(NodesterDBContext context) : base(context)
        {
        }

        public IEnumerable<MacroDto> GetMacrosAssignedToUser(Guid userId)
        {
            var macros = GetAll(x => x.UserId == userId).OrderBy(x => x.CreatedOn);
            return macros
                .Select(x => x.Adapt<MacroDto>());
        }

        public async Task<MacroDto> GetById(Guid id)
        {
            var macro = await GetAsync(id);
            return macro.Adapt<MacroDto>();
        }

        public MacroDto CreateNewMacro(CreateMacroDto newMacro)
        {
            var macro = newMacro.Adapt<Macro>();
            macro.IsActive = true;
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