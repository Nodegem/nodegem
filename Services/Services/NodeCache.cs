using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Core.Nodes.Graph;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Definitions;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Services
{
    public static class NodeCache
    {
        private static List<NodeDefinition> _allNodeDefinitions;
        private static Dictionary<Type, Type[]> _nodeTypeMapper;
        private static Dictionary<string, Type> _nodeCategoryTypeMapper;

        public static IReadOnlyDictionary<string, Type> NodeCategoryTypeMapper => _nodeCategoryTypeMapper;
        public static IReadOnlyList<NodeDefinition> AllNodeDefinitions => _allNodeDefinitions;

        public static IReadOnlyList<NodeDefinition> NonListenerNodeDefinitions =>
            _allNodeDefinitions.Where(x => !x.IsListenerOnly).ToList();

        public static void CacheNodeData(IServiceProvider provider)
        {
            var allNodeTypes = RetrieveAllNodeTypes().ToList();
            _nodeTypeMapper = allNodeTypes.ToDictionary(k => k, GetNodeTypeMap);
            _nodeCategoryTypeMapper = _nodeTypeMapper
                .ToDictionary(k => CreateKey(k.Key), v => v.Key);
            _allNodeDefinitions = allNodeTypes.Select(x => GetDefinitionFromNode(x, provider)).ToList();

            //Check for unique node ids
            try
            {
                var _ = _allNodeDefinitions.ToDictionary(k => k.Id, v => v);
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Multiple nodes with same id's detected. Message: {ex.Message}");
            }
        }

        private static string CreateKey(Type type)
        {
            return type.GetAttributeValue((DefinedNodeAttribute nc) => nc.Id);
        }

        private static IEnumerable<Type> RetrieveAllNodeTypes()
        {
            var assemblies = GetAssemblies();
            var assemblyTypes = assemblies.SelectMany(x => x.GetTypes()).ToList();
            assemblyTypes.RemoveAll(FilterUnwantedTypes);
            return assemblyTypes;
        }

        private static bool FilterUnwantedTypes(Type nodeType)
        {
            if (nodeType.IsAbstract || nodeType.IsInterface || nodeType.ContainsGenericParameters ||
                nodeType.IsValueType) return true;
            var definedNode = nodeType.GetAttributeValue((DefinedNodeAttribute dn) => dn);
            return definedNode == null || definedNode.Ignore;
        }

        public static INode BuildNodeFromTypeMap(Type nodeType, IServiceProvider serviceProvider)
        {
            var @params = _nodeTypeMapper[nodeType];
            return (INode) Activator.CreateInstance(nodeType,
                @params
                    .Select(serviceProvider.GetService).ToArray());
        }

        public static bool IsConstant(string definitionId)
        {
            return definitionId == GetConstant.ConstantDefinitionId;
        }

        private static NodeDefinition GetDefinitionFromNode(Type nodeType, IServiceProvider provider)
        {
            return BuildNodeFromTypeMap(nodeType, provider).GetDefinition();
        }

        private static Type[] GetNodeTypeMap(Type nodeType)
        {
            return GetNodeParams(nodeType);
        }

        private static Type[] GetNodeParams(Type nodeType)
        {
            var constructorParams = nodeType.GetConstructors().Select(x => x.GetParameters());
            var constructorWithParams = constructorParams.FirstOrDefault(x => x.Any());
            return constructorWithParams != null
                ? constructorWithParams.Select(x => x.ParameterType).ToArray()
                : new Type[0];
        }
        
        private static IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}