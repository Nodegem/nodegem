using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodester.Common.Data;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Nodes.Graph;
using Nodester.Services.Data;

namespace Nodester.Services.Extensions
{
    public static class HelperExtensions
    {
        public static async Task<Dictionary<Guid, INode>> ToNodeDictionaryAsync(this IEnumerable<NodeDto> nodes,
            IServiceProvider provider, IMacroManagerService macroManager, User user, IDictionary<Guid, Constant> constants = null)
        {
            if (nodes == null) return new Dictionary<Guid, INode>();

            var keyValArray =
                await Task.WhenAll(nodes.Where(x => x.MacroFieldId == null)
                    .Select(async p =>
                        new KeyValuePair<Guid, INode>(p.Id, await BuildNode(p, user, macroManager, provider, constants))));

            return keyValArray.ToDictionary(k => k.Key, v => v.Value);
        }

        private static async Task<INode> BuildNode(NodeDto nodeDto, User user, IMacroManagerService macroManager,
            IServiceProvider provider, IDictionary<Guid, Constant> constants)
        {
            INode node;

            try
            {
                if (nodeDto.MacroId.HasValue)
                {
                    var macro = await macroManager.BuildMacroAsync(user, nodeDto.MacroId.Value);
                    macro.PopulateInputsWithNewDefaults(nodeDto.FieldData);
                    node = macro.ToMacroNode();
                }
                else
                {
                    node = NodeCache.BuildNodeFromTypeMap(NodeCache.NodeCategoryTypeMapper[nodeDto.FullName], provider);
                    node.PopulateWithData(nodeDto.FieldData);
                }
            }
            catch (KeyNotFoundException)
            {
                var lowerName = nodeDto.FullName.ToLower();
                if (lowerName.Contains("constants."))
                {
                    var key = constants
                        .FirstOrDefault(c => lowerName.Contains(c.Value.Label.ToLower())).Key;
                    node = new GetConstant(key);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            node.SetId(nodeDto.Id);
            return node;
        }
    }
}