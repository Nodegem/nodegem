using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GraphController> _logger;
        
        public GraphController(
            IGraphRepository graphRepo,
            ILogger<GraphController> logger)
        {
            _graphRepo = graphRepo;
            _logger = logger;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<GraphDto>> GetAllGraphs()
        {
            try
            {
                return Ok(_graphRepo.GetGraphsAssignedToUser(User.GetUserId()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to retrieve graphs");
                return BadRequest("Something went wrong");
            }
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