using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Common.Data;
using Nodester.Common.Dto.ComponentDtos;
using Nodester.Common.Extensions;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Nodes.Graph;
using Nodester.Services;

namespace Nodester.Bridge.Extensions
{
    public static class NodeExtensions
    {
        public static async Task<Dictionary<Guid, INode>> ToNodeDictionaryAsync(this IEnumerable<NodeDto> nodes,
            IEnumerable<LinkDto> links,
            IServiceProvider provider, IBuildMacroService macroService, User user,
            IDictionary<Guid, Constant> constants = null)
        {
            if (nodes == null) return new Dictionary<Guid, INode>();

            var keyValArray =
                await nodes.Where(x => x.MacroFieldId == null)
                    .SelectAsync(async p =>
                        new KeyValuePair<Guid, INode>(p.Id,
                            await BuildNode(p, links.Where(l => l.DestinationNode == p.Id || l.SourceNode == p.Id),
                                user, macroService, provider, constants)));

            return keyValArray.ToDictionary(k => k.Key, v => v.Value);
        }

        private static async Task<INode> BuildNode(NodeDto nodeDto, IEnumerable<LinkDto> links, User user,
            IBuildMacroService macroService,
            IServiceProvider provider, IDictionary<Guid, Constant> constants)
        {
            INode node;

            try
            {
                if (nodeDto.MacroId.HasValue)
                {
                    var macro = await macroService.BuildMacroAsync(user, nodeDto.MacroId.Value);
                    macro.PopulateInputsWithNewDefaults(nodeDto.FieldData);
                    node = macro.ToMacroNode();
                }
                else
                {
                    node = NodeCache.BuildNodeFromTypeMap(NodeCache.NodeCategoryTypeMapper[nodeDto.FullName], provider);
                    var indefiniteFields = GetIndefiniteFieldKeyValuePairs(links);
                    if (nodeDto.FieldData.Any())
                    {
                        indefiniteFields = indefiniteFields.Concat(nodeDto.FieldData.Select(x =>
                            new KeyValuePair<string, string>(x.Key.Split('|')[0], x.Key))).Distinct();
                    }
                    node.PopulateIndefinites(indefiniteFields);
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

            node.SetId(nodeDto.Id);
            return node;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetIndefiniteFieldKeyValuePairs(
            IEnumerable<LinkDto> links)
            => links.Where(x => x.DestinationKey.Contains('|') || x.SourceKey.Contains('|'))
                .Select(x => x.DestinationKey.Contains('|') ? x.DestinationKey.Split('|') : x.SourceKey.Split('|'))
                .Select(x => new KeyValuePair<string, string>(x[0].ToLower(), $"{x[0]}|{x[1]}"));
    }
}