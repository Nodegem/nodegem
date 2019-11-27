using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Core.Extensions;
using Nodegem.Engine.Core.Fields.Graph;
using Nodegem.Engine.Core.Utils;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Definitions;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Links;
using Nodegem.Engine.Data.Nodes;
using ValueType = Nodegem.Common.Data.ValueType;

namespace Nodegem.Engine.Core
{
    public abstract class Node : INode
    {
        public virtual IGraph Graph { get; private set; }
        private Type Type { get; }
        private string Title => GetNodeTitle();
        private string Namespace => Type.GetAttributeValue((NodeNamespaceAttribute nc) => nc.Namespace);

        public Guid Id { get; private set; }
        private IList<IFlowInputField> FlowInputs { get; set; }
        private IList<IFlowOutputField> FlowOutputs { get; set; }
        private IList<IValueInputField> ValueInputs { get; set; }
        private IList<IValueOutputField> ValueOutputs { get; set; }
        private IDictionary<string, Action<string>> IndefiniteFields { get; set; }

        public IEnumerable<IValueLink> ValueConnections =>
            FieldMap.Values.OfType<IValueInputField>().Where(x => x.Connection != null).Select(x => x.Connection);

        public IEnumerable<IFlowLink> FlowConnections =>
            FieldMap.Values.OfType<IFlowOutputField>().Where(x => x.Connection != null).Select(x => x.Connection);

        private IDictionary<string, IField> FieldMap { get; set; }

        protected Node(bool shouldInitialize = true)
        {
            Type = GetType();

            if (shouldInitialize)
            {
                Initialize();
            }
        }

        public IField GetFieldByKey(string key)
        {
            if (!FieldMap.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Node does not contain field with key: {key}");
            }

            return FieldMap[key];
        }

        protected void Initialize()
        {
            FlowInputs = new List<IFlowInputField>();
            FlowOutputs = new List<IFlowOutputField>();
            ValueInputs = new List<IValueInputField>();
            ValueOutputs = new List<IValueOutputField>();
            IndefiniteFields = new Dictionary<string, Action<string>>();

            Define();

            AggregateFields();
        }

        public INode PopulateWithData(IEnumerable<FieldData> fields)
        {
            MapDataToIo(fields);
            return this;
        }

        public INode PopulateIndefinites(IEnumerable<KeyValuePair<string, string>> indefiniteKeyValuePairs)
        {
            if (!indefiniteKeyValuePairs.Any()) return this;
            indefiniteKeyValuePairs.ForEach(i =>
            {
                var (key, value) = i;
                if (!FieldMap.ContainsKey(value) && IndefiniteFields.ContainsKey(key))
                {
                    IndefiniteFields[key].Invoke(value);
                }
            });
            AggregateFields();

            return this;
        }

        public INode SetId(Guid id)
        {
            Id = id;
            return this;
        }

        public INode SetGraph(IGraph graph)
        {
            Graph = graph;
            return this;
        }

        protected abstract void Define();

        private void MapDataToIo(IEnumerable<FieldData> fields)
        {
            var fieldList = fields.ToList();
            var inputDict = ValueInputs.ToDictionary(k => k.Key, v => v);
            var outputDict = ValueOutputs.ToDictionary(k => k.Key, v => v);
            fieldList.ForEach(x =>
            {
                if (inputDict.ContainsKey(x.Key))
                {
                    inputDict[x.Key].SetValue(x.Value);
                }

                if (outputDict.ContainsKey(x.Key))
                {
                    outputDict[x.Key].SetValue(x.Value);
                }
            });
        }

        public virtual NodeDefinition GetDefinition()
        {
            var fieldLabels = GetFieldLabels();
            var nodeId = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.Id);

            if (string.IsNullOrEmpty(nodeId) || !Guid.TryParse(nodeId, out _))
            {
                throw new Exception($"Invalid node ID. Node ID must be present and be a GUID.");
            }
            
            var definition = new NodeDefinition
            {
                Id = nodeId,
                FullName = $"{Namespace}.{Type.Name}",
                Title = Title,
                Description = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.Description),
                IgnoreDisplay = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.IgnoreDisplay),
                IsListenerOnly = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.IsListenerOnly),
                FlowInputs = FlowInputs.Select(x => x.ToFlowInputDefinition(fieldLabels[x.Key].Label)).ToList(),
                FlowOutputs = FlowOutputs.Select(x =>
                    x.ToFlowOutputDefinition(fieldLabels[x.Key].Label, fieldLabels[x.Key].Indefinite)).ToList(),
                ValueInputs = ValueInputs
                    .Select(x => x.ToValueInputDefinition(fieldLabels[x.Key].Label, fieldLabels[x.Key].Type,
                        fieldLabels[x.Key].Indefinite, fieldLabels[x.Key].IsEditable,
                        fieldLabels[x.Key].AllowConnection)).ToList(),
                ValueOutputs = ValueOutputs
                    .Select(x => x.ToValueOutputDefinition(fieldLabels[x.Key].Label, fieldLabels[x.Key].Type,
                        fieldLabels[x.Key].Indefinite)).ToList()
            };

            return definition;
        }

        protected FlowInput AddFlowInput(string key, Func<IFlow, Task<IFlowOutputField>> action)
        {
            var input = new FlowInput(key, action);
            FlowInputs.Add(input);
            return input;
        }

        protected FlowOutput AddFlowOutput(string key)
        {
            var output = new FlowOutput(key);
            FlowOutputs.Add(output);
            return output;
        }

        protected ValueInput AddValueInput<T>(string key, T @default = default)
        {
            var input = new ValueInput(key, @default, typeof(T));
            ValueInputs.Add(input);
            return input;
        }

        protected IEnumerable<ValueInput> InitializeValueInputList<T>(string baseKey, T @default = default,
            int amount = 1)
        {
            // Initial "keys" will always be numeric but anything new will be a GUID
            var inputs = Enumerable.Range(0, amount)
                .Select((x, i) => new ValueInput($"{baseKey}|{i}", @default, typeof(T)))
                .ToList();
            inputs.ForEach(ValueInputs.Add);
            IndefiniteFields.Add(baseKey.ToLower(),
                key =>
                {
                    var newInput = new ValueInput(key, @default, typeof(T));
                    inputs.Add(newInput);
                    ValueInputs.Add(newInput);
                });
            return inputs;
        }

        protected IEnumerable<ValueOutput> InitializeValueOutputList<T>(string baseKey,
            Func<IFlow, string, Task<object>> valueFunc,
            int amount = 1)
        {
            // Initial "keys" will always be numeric but anything new will be a GUID
            var outputs = Enumerable.Range(0, amount)
                .Select((x, i) =>
                    new ValueOutput($"{baseKey.ToLower()}|{i}", flow => valueFunc(flow, $"{baseKey.ToLower()}|{i}"),
                        typeof(T)))
                .ToList();
            outputs.ForEach(ValueOutputs.Add);
            IndefiniteFields.Add(baseKey.ToLower(),
                key =>
                {
                    var newValueOutput = new ValueOutput(key, flow => valueFunc(flow, key), typeof(T));
                    outputs.Add(newValueOutput);
                    ValueOutputs.Add(newValueOutput);
                });
            return outputs;
        }

        protected IEnumerable<FlowOutput> InitializeFlowOutputList(string baseKey,
            int amount = 1)
        {
            // Initial "keys" will always be numeric but anything new will be a GUID
            var outputs = Enumerable.Range(0, amount)
                .Select((x, i) => new FlowOutput($"{baseKey}|{i}"))
                .ToList();
            outputs.ForEach(FlowOutputs.Add);
            IndefiniteFields.Add(baseKey.ToLower(),
                key =>
                {
                    var newFlowOutput = new FlowOutput(key);
                    outputs.Add(newFlowOutput);
                    FlowOutputs.Add(newFlowOutput);
                });
            return outputs;
        }

        protected ValueOutput AddValueOutput<T>(string key)
        {
            var output = new ValueOutput(key, typeof(T));
            ValueOutputs.Add(output);
            return output;
        }

        protected ValueOutput AddValueOutput<T>(string key, Func<IFlow, Task<T>> valueFunc)
        {
            return AddValueOutput(key, async flow => await valueFunc(flow), typeof(T));
        }

        private ValueOutput AddValueOutput(string key, Func<IFlow, Task<object>> valueFunc, Type type)
        {
            var output = new ValueOutput(key, valueFunc, type);
            ValueOutputs.Add(output);
            return output;
        }

        private string GetNodeTitle()
        {
            var value = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.Title);
            return string.IsNullOrEmpty(value) ? Type.Name.SplitOnCapitalLetters() : value;
        }

        private void AggregateFields()
        {
            var allFields = EnumerableHelper.Concat<IField>(FlowInputs, FlowOutputs, ValueInputs, ValueOutputs)
                .ToList();
            FieldMap = allFields.ToDictionary(k => k.Key, v => v);
        }

        private IDictionary<string, FieldInfo> GetFieldLabels()
        {
            return Type.GetProperties()
                .Where(p =>
                {
                    var pType = p.PropertyType;
                    var interfaces = pType.GetInterfaces();
                    if (!interfaces.Contains(typeof(IField)) && !typeof(IEnumerable).IsAssignableFrom(pType))
                    {
                        return false;
                    }

                    if (interfaces.Contains(typeof(IField)))
                    {
                        return true;
                    }

                    if (pType.IsGenericType && pType.GenericTypeArguments.Any())
                    {
                        return pType.GenericTypeArguments.Any(gta => gta.GetInterfaces().Contains(typeof(IField)));
                    }

                    return false;
                })
                .SelectMany(pi =>
                {
                    var field = pi.GetValue<IField>(this);
                    var indefinite = field == null;

                    var label = field?.OriginalName.SplitOnCapitalLetters().ToTitleCase();
                    var fieldAttributes = pi.GetCustomAttribute<FieldAttributesAttribute>() ??
                                          new FieldAttributesAttribute(label);
                    fieldAttributes.Label ??= label;
                    var key = field?.Key ?? fieldAttributes.Key;

                    if (indefinite)
                    {
                        var fieldList = pi.GetValue<IEnumerable<IField>>(this);
                        return fieldList.Select(f => new FieldInfo
                        {
                            Key = f.Key,
                            Indefinite = true,
                            Label = fieldAttributes.Label,
                            Type = fieldAttributes.Type ??
                                   (field is IValueField vField ? vField.ValueType : ValueType.Any),
                            IsEditable = fieldAttributes.IsEditable,
                            AllowConnection = fieldAttributes.AllowConnection
                        });
                    }

                    return new List<FieldInfo>
                    {
                        new FieldInfo
                        {
                            Key = key,
                            Label = label ?? fieldAttributes.Label,
                            Indefinite = false,
                            Type = fieldAttributes.Type ??
                                   (field is IValueField valueField ? valueField.ValueType : ValueType.Any),
                            IsEditable = fieldAttributes.IsEditable,
                            AllowConnection = fieldAttributes.AllowConnection
                        }
                    };
                })
                .ToDictionary(
                    x => x.Key,
                    v => v);
        }

        public virtual ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        private struct FieldInfo
        {
            public string Key { get; set; }
            public string Label { get; set; }
            public bool Indefinite { get; set; }
            public ValueType Type { get; set; }
            public bool IsEditable { get; set; }
            public bool AllowConnection { get; set; }
        }
    }
}