using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Data.Contexts;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;
using Nodester.Services.Data.Mappers;
using Nodester.Services.Data.Repositories;

namespace Nodester.Services.Repositories
{
    public class MacroRepository : Repository<Macro>, IMacroRepository
    {
        private IMapper<Macro, CreateMacroDto> _createMacroMapper;
        private IMapper<Macro, MacroDto> _macroMapper;

        public MacroRepository(NodesterDBContext context,
            IMapper<Macro, CreateMacroDto> createMacroMapper,
            IMapper<Macro, MacroDto> macroMapper) : base(context)
        {
            _createMacroMapper = createMacroMapper;
            _macroMapper = macroMapper;
        }


        public IEnumerable<MacroDto> GetAllMacros(Guid userId)
        {
            return GetAll(x => x.UserId == userId).Select(x => _macroMapper.ToDto(x));
        }

        public async Task<MacroDto> GetById(Guid id)
        {
            return _macroMapper.ToDto(await GetAsync(id));
        }

        public MacroDto CreateNewMacro(CreateMacroDto newMacro)
        {
            var macro = _createMacroMapper.ToModel(newMacro);
            Create(macro);
            return _macroMapper.ToDto(macro);
        }

        public MacroDto UpdateMacro(Guid macroId, MacroDto macro)
        {
            var updatedMacro = _macroMapper.ToModel(macro);
            Update(macroId, updatedMacro);
            return _macroMapper.ToDto(updatedMacro);
        }

        public void DeleteMacro(Guid macroId)
        {
            Delete(macroId);
        }
    }
}