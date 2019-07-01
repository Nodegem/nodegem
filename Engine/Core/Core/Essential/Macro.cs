using Nodester.Common.Extensions;
using Nodester.Graph.Core.Data;
using Nodester.Graph.Core.Data.Attributes;
using Nodester.Graph.Core.Data.Fields;

namespace Nodester.Graph.Core.Essential
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
            _macroGraph.FlowInputs.ForEach(x => AddFlowInput(x.Key, flow => ExecuteMacro(x)));
            _macroGraph.FlowOutputs.ForEach(x =>
            {
                 x.SetParentGraph(() => Graph as IFlowGraph);
                AddFlowOutput(x.Key);
            });
            _macroGraph.ValueInputs.ForEach(x => AddValueInput(x.Key, x.DefaultValue));
            _macroGraph.ValueOutputs.ForEach(x => AddValueOutput(x.Key, flow => GetOutput(x)));
        }

        private IFlowOutputField ExecuteMacro(IMacroFlowInputField input)
        {
            return _macroGraph.Execute(input);
        }

        private object GetOutput(IMacroValueOutputField output)
        {
            return _macroGraph.GetValue<object>(output);
        }
    }
}