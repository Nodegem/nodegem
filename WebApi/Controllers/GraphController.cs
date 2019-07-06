using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nodester.Common.Extensions;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Services.Data.Repositories;

namespace Nodester.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GraphController : ControllerBase
    {
        private readonly IGraphRepository _graphRepo;

        public GraphController(
            IGraphRepository graphRepo)
        {
            _graphRepo = graphRepo;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<GraphDto>> GetAllGraphs()
        {
            return Ok(_graphRepo.GetAllGraphsByUser(User.GetUserId()));
        }

        [HttpGet("{graphId}")]
        public async Task<ActionResult<GraphDto>> GetGraph(Guid graphId)
        {
            return Ok(await _graphRepo.GetGraphAsync(graphId));
        }

        [HttpPost("create")]
        public ActionResult<GraphDto> CreateGraph([FromBody] CreateGraphDto graph)
        {
            return _graphRepo.CreateGraph(graph);
        }

        [HttpPut("update")]
        public ActionResult<GraphDto> Update([FromBody] GraphDto graph)
        {
            return _graphRepo.UpdateGraph(graph);
        }

        [HttpDelete("{graphId}")]
        public ActionResult DeleteGraph(Guid graphId)
        {
            _graphRepo.DeleteGraph(graphId);
            return Ok();
        }
    }
}