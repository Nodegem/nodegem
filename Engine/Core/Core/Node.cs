using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Definitions;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Links;
using Nodester.Engine.Data.Nodes;
using Nodester.Graph.Core.Extensions;
using Nodester.Graph.Core.Fields.Graph;
using Nodester.Graph.Core.Utils;
using ValueType = Nodester.Engine.Data.ValueType;

namespace Nodester.Graph.Core
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
            fieldList.ForEach(f =>
            {
                if (!f.Key.Contains('|') || FieldMap.ContainsKey(f.Key)) return;
                var @base = f.Key.Split('|')[0].ToLower();
                if (IndefiniteFields.ContainsKey(@base))
                {
                    IndefiniteFields[@base].Invoke(f.Key);
                }
            });
            var inputDict = ValueInputs.ToDictionary(k => k.Key, v => v);
            fieldList.ForEach(x => inputDict[x.Key].SetValue(x.Value));
        }

        public virtual NodeDefinition GetDefinition()
        {
            var fieldLabels = GetFieldLabels();
            return new NodeDefinition
            {
                FullName = $"{Namespace}.{Type.Name}",
                Title = Title,
                Description = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.Description),
                IgnoreDisplay = Type.GetAttributeValue((DefinedNodeAttribute dn) => dn.IgnoreDisplay),
                FlowInputs = FlowInputs.Select(x => x.ToFlowInputDefinition(fieldLabels[x.Key].Label)).ToList(),
                FlowOutputs = FlowOutputs.Select(x => x.ToFlowOutputDefinition(fieldLabels[x.Key].Label)).ToList(),
                ValueInputs = ValueInputs
                    .Select(x => x.ToValueInputDefinition(fieldLabels[x.Key].Label, fieldLabels[x.Key].Type,
                        fieldLabels[x.Key].Indefinite)).ToList(),
                ValueOutputs = ValueOutputs
                    .Select(x => x.ToValueOutputDefinition(fieldLabels[x.Key].Label, fieldLabels[x.Key].Type)).ToList()
            };
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
                                   (field is IValueField vField ? vField.ValueType : ValueType.Any)
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
                                   (field is IValueField valueField ? valueField.ValueType : ValueType.Any)
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
            public Engine.Data.ValueType Type { get; set; }
        }
    }
}