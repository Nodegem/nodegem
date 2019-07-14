using System.Threading.Tasks;
using Nodester.Common.Extensions;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Essential
{
    [DefinedNode("Macro", Ignore = true)]
    [NodeNamespace("Core.Essential")]
    public class Macro : Node
    {
        private readonly Nodester.Graph.Core.Macro.MacroGraph _macroGraph;

        public Macro(Nodester.Graph.Core.Macro.MacroGraph macroGraph) : base(false)
        {
            _macroGraph = macroGraph;
            Initialize();
        }

        protected override void Define()
        {
            _macroGraph.FlowInputs.ForEach(x => AddFlowInput(x.Key, flow => ExecuteMacroAsync(x)));
            _macroGraph.FlowOutputs.ForEach(x =>
            {
                 x.SetParentGraph(() => Graph as IFlowGraph);
                AddFlowOutput(x.Key);
            });
            _macroGraph.ValueInputs.ForEach(x => AddValueInput(x.Key, x.DefaultValue));
            _macroGraph.ValueOutputs.ForEach(x => AddValueOutput<object>(x.Key, flow => GetOutputAsync(x)));
        }

        private Task<IFlowOutputField> ExecuteMacroAsync(IMacroFlowInputField input)
        {
            _macroGraph.IsRunningLocally = Graph.IsRunningLocally;
            return _macroGraph.ExecuteAsync(input);
        }

        private async Task<object> GetOutputAsync(IMacroValueOutputField output)
        {
            return await _macroGraph.GetValueAsync<object>(output);
        }
    }
}