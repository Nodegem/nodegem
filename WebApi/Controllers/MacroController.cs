using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nodegem.Common.Dto;
using Nodegem.Common.Extensions;
using Nodegem.Data.Dto.MacroDtos;
using Nodegem.Services.Data.Repositories;

namespace Nodegem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MacroController : ControllerBase
    {
        private readonly IMacroRepository _macroRepository;

        public MacroController(IMacroRepository macroRepository)
        {
            _macroRepository = macroRepository;
        }

        [HttpGet("all")]
        public IEnumerable<MacroDto> GetAllMacros()
        {
            return _macroRepository.GetMacrosAssignedToUser(User.GetUserId());
        }

        [HttpGet("{macroId}")]
        public async Task<MacroDto> GetMacro(Guid macroId)
        {
            return await _macroRepository.GetById(macroId);
        }

        [HttpPost("create")]
        public MacroDto CreateMacro([FromBody] CreateMacroDto createMacroDto)
        {
            return _macroRepository.CreateNewMacro(createMacroDto);
        }

        [HttpPut("update/{macroId}")]
        public MacroDto UpdateMacro(Guid macroId, [FromBody] MacroDto macroDto)
        {
            return _macroRepository.UpdateMacro(macroId, macroDto);
        }

        [HttpDelete("{macroId}")]
        public ActionResult DeleteMacro(Guid macroId)
        {
            _macroRepository.DeleteMacro(macroId);
            return Ok();
        }
    }
}