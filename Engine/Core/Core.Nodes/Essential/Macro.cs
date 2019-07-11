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
            _macroGraph.FlowInputs.ForEach(x => AddFlowInput(x.Key, flow => ExecuteMacro(x, flow.IsRunningLocally)));
            _macroGraph.FlowOutputs.ForEach(x =>
            {
                 x.SetParentGraph(() => Graph as IFlowGraph);
                AddFlowOutput(x.Key);
            });
            _macroGraph.ValueInputs.ForEach(x => AddValueInput(x.Key, x.DefaultValue));
            _macroGraph.ValueOutputs.ForEach(x => AddValueOutput(x.Key, flow => GetOutput(x)));
        }

        private Task<IFlowOutputField> ExecuteMacro(IMacroFlowInputField input, bool isLocal = true)
        {
            return _macroGraph.ExecuteAsync(input, isLocal);
        }

        private object GetOutput(IMacroValueOutputField output)
        {
            return _macroGraph.GetValue<object>(output);
        }
    }
}