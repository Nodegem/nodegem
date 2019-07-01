using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nodester.Common.Extensions;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Services.Data.Repositories;

namespace Nodester.WebApi.Controllers
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
        public ActionResult<IEnumerable<MacroDto>> GetAllMacros()
        {
            return Ok(_macroRepository.GetAllMacros(User.GetUserId()));
        }

        [HttpGet("{macroId}")]
        public ActionResult<MacroDto> GetMacro(Guid macroId)
        {
            return Ok(_macroRepository.GetById(macroId));
        }

        [HttpPost("create")]
        public ActionResult<MacroDto> CreateMacro([FromBody] CreateMacroDto createMacroDto)
        {
            return Ok(_macroRepository.CreateNewMacro(createMacroDto));
        }

        [HttpPut("update/{macroId}")]
        public ActionResult<MacroDto> UpdateMacro(Guid macroId, [FromBody] MacroDto macroDto)
        {
            return Ok(_macroRepository.UpdateMacro(macroId, macroDto));
        }

        [HttpDelete("{macroId}")]
        public ActionResult DeleteMacro(Guid macroId)
        {
            _macroRepository.DeleteMacro(macroId);
            return Ok();
        }
    }
}