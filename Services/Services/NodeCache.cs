using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodester.Common.Extensions;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Definitions;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Services
{
    public static class NodeCache
    {
        private static List<NodeDefinition> _nodeDefinitions;
        private static Dictionary<Type, Type[]> _nodeTypeMapper;
        private static Dictionary<string, Type> _nodeCategoryTypeMapper;

        public static IReadOnlyDictionary<string, Type> NodeCategoryTypeMapper => _nodeCategoryTypeMapper;
        public static IReadOnlyList<NodeDefinition> NodeDefinitions => _nodeDefinitions;

        public static void CacheNodeData(IServiceProvider provider, string projectName)
        {
            var allNodeTypes = RetrieveAllNodeTypes(projectName).ToList();
            _nodeTypeMapper = allNodeTypes.ToDictionary(k => k, GetNodeTypeMap);
            _nodeCategoryTypeMapper = _nodeTypeMapper
                .ToDictionary(k => CreateKey(k.Key), v => v.Key);
            _nodeDefinitions = allNodeTypes.Select(x => GetDefinitionFromNode(x, provider)).ToList();
        }

        private static string CreateKey(Type type)
        {
            var @namespace = type.GetAttributeValue((NodeNamespaceAttribute nc) => nc.Namespace);
            var name = type.Name;
            return $"{@namespace}.{name}";
        }

        private static IEnumerable<Type> RetrieveAllNodeTypes(string projectName)
        {
            var assemblies = GetAssemblies(projectName);
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
        
        private static IEnumerable<Assembly> GetAssemblies(string projectName)
        {
            var test = AppDomain.CurrentDomain.GetAssemblies();
            return AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith(projectName));
        }
    }
}