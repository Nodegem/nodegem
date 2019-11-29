using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nodegem.Common.Dto;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Common.Dto.FlowFieldDtos;
using Nodegem.Common.Dto.ValueFieldDtos;
using Nodegem.Common.Extensions;
using Nodegem.Data;
using Nodegem.Engine.Core.Nodes.Graph;
using Nodegem.Engine.Core.Utils;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Definitions;
using Nodegem.Services;
using Nodegem.Services.Data;
using Nodegem.Services.Data.Repositories;
using Macro = Nodegem.Engine.Core.Nodes.Essential.Macro;

namespace Nodegem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NodesController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGraphRepository _graphRepo;
        private readonly IMacroRepository _macroRepo;
        private readonly ILogger<NodesController> _logger;

        public NodesController(IUserService userService, IGraphRepository graphRepo, IMacroRepository macroRepo,
            ILogger<NodesController> logger)
        {
            _userService = userService;
            _graphRepo = graphRepo;
            _macroRepo = macroRepo;
            _logger = logger;
        }

        [HttpGet("definitions/{type}/{graphId}")]
        public async Task<ActionResult<NamespaceNodeDefinition>> GetNodeDefinitions(GraphType type, Guid graphId)
        {
            try
            {
                var userId = User.GetUserId();
                var macros = _macroRepo.GetMacrosAssignedToUser(userId).ToList();
                var userConstants = (await _userService.GetConstantsAsync(userId)).ToList();
                var graphConstants = type == GraphType.Graph
                    ? await _graphRepo.GetConstantsAsync(graphId)
                    : new List<ConstantDto>();

                var isListener = type == GraphType.Graph && await _graphRepo.IsListenerGraphAsync(graphId);
                var defaultNodeDefinitions =
                    isListener ? NodeCache.AllNodeDefinitions : NodeCache.NonListenerNodeDefinitions;

                var macroFieldDefinitions = new List<NodeDefinition>();
                if (type == GraphType.Macro)
                {
                    defaultNodeDefinitions = defaultNodeDefinitions
                        .Where(x => !x.FullName.ToLower().Contains("essential"))
                        .ToList();
                    var macro = macros.First(x => x.Id == graphId);
                    var macroFields = EnumerableHelper
                        .Concat<BaseFieldDto>(macro.FlowInputs, macro.FlowOutputs, macro.ValueInputs,
                            macro.ValueOutputs)
                        .ToList();
                    macroFieldDefinitions = ConvertMacroFieldsToNodeDefinitions(macroFields).ToList();
                }

                var macroDefinitions = ConvertMacrosToDefinitions(macros);
                var userConstantDefinitions = ConvertConstantsToNodeDefinitions(userConstants, "User");
                var graphConstantDefinitions = ConvertConstantsToNodeDefinitions(graphConstants, "Graph");

                var allDefinitions = EnumerableHelper.Concat(defaultNodeDefinitions, macroFieldDefinitions,
                    macroDefinitions, userConstantDefinitions, graphConstantDefinitions).ToList();

                var hierarchicalNode = new NamespaceNodeDefinition();
                foreach (var definition in allDefinitions)
                {
                    var ns = definition.FullName.Split('.');
                    hierarchicalNode.AddObject(ns.Slice(0, ns.Length - 1), definition);
                }

                return Ok(hierarchicalNode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building namespace node definition");
                return BadRequest();
            }
        }

        private static IEnumerable<NodeDefinition> ConvertMacroFieldsToNodeDefinitions(IEnumerable<BaseFieldDto> fields)
        {
            return fields.Select(f =>
            {
                var type = f is ValueFieldDto ? "Value" : "Flow";
                var titlePrefix = f is ValueOutputDto || f is FlowOutputDto ? "Set" : "Get";
                var definition = new NodeDefinition
                {
                    Id = Macro.MacroFieldDefinitionId,
                    Title = $"{titlePrefix} {f.Label}",
                    FullName = $"Macros.Fields.{type}.{f.Label}",
                    MacroFieldId = f.Key,
                };

                switch (f)
                {
                    case FlowInputDto fi:
                        definition.FlowOutputs = new List<FlowOutputDefinition>
                        {
                            new FlowOutputDefinition
                            {
                                Key = f.Key.ToString(),
                                Label = f.Label
                            }
                        };
                        break;
                    case FlowOutputDto fo:
                        definition.FlowInputs = new List<FlowInputDefinition>
                        {
                            new FlowInputDefinition
                            {
                                Key = f.Key.ToString(),
                                Label = f.Label
                            }
                        };
                        break;
                    case ValueInputDto vi:
                        definition.ValueOutputs = new List<ValueOutputDefinition>
                        {
                            new ValueOutputDefinition
                            {
                                Key = f.Key.ToString(),
                                Label = f.Label
                            }
                        };
                        break;
                    case ValueOutputDto vo:
                        definition.ValueInputs = new List<ValueInputDefinition>
                        {
                            new ValueInputDefinition
                            {
                                Key = f.Key.ToString(),
                                Label = f.Label
                            }
                        };
                        break;
                }

                return definition;
            });
        }

        private static IEnumerable<NodeDefinition> ConvertConstantsToNodeDefinitions(IEnumerable<ConstantDto> constants,
            string path)
        {
            return constants.Select(c => new NodeDefinition
            {
                Id = $"{GetConstant.ConstantDefinitionId}|{c.Key}",
                Title = $"Get {c.Label}",
                FullName = $"Constants.{path}.{c.Label}",
                ConstantId = c.Key,
                ValueOutputs = new List<ValueOutputDefinition>
                {
                    new ValueOutputDefinition
                    {
                        Key = GetConstant.ValueKey.ToLower(),
                        Label = c.Label,
                        ValueType = c.Type
                    }
                }
            });
        }

        private static IEnumerable<NodeDefinition> ConvertMacrosToDefinitions(IEnumerable<MacroDto> macros)
        {
            return macros.Select(m => new NodeDefinition
            {
                Id = Macro.MacroDefinitionId, 
                Title = m.Name,
                Description = m.Description,
                FullName = $"Macros.Custom.{m.Name}",
                MacroId = m.Id,
                FlowInputs = m.FlowInputs.Select(fi => new FlowInputDefinition
                {
                    Key = fi.Key.ToString(),
                    Label = fi.Label
                }).ToList(),
                FlowOutputs = m.FlowOutputs.Select(fo => new FlowOutputDefinition
                {
                    Key = fo.Key.ToString(),
                    Label = fo.Label
                }).ToList(),
                ValueInputs = m.ValueInputs.Select(vi => new ValueInputDefinition
                {
                    Key = vi.Key.ToString(),
                    Label = vi.Label,
                    DefaultValue = vi.DefaultValue,
                    ValueType = vi.Type
                }).ToList(),
                ValueOutputs = m.ValueOutputs.Select(vo => new ValueOutputDefinition
                {
                    Key = vo.Key.ToString(),
                    Label = vo.Label,
                    ValueType = vo.Type
                }).ToList()
            });
        }
    }
}