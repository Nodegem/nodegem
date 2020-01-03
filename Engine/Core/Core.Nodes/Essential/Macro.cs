using System.Threading.Tasks;
using Nodegem.Common.Extensions;
using Nodegem.Engine.Core.Macro;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Essential
{
    [DefinedNode(MacroDefinitionId, Ignore = true)]
    [NodeNamespace("Core.Essential")]
    public class Macro : Node
    {

        public const string MacroDefinitionId = "968747E3-88FC-4DFE-8A59-E2D6B3D9D3B8"; 
        public const string MacroFieldDefinitionId = "E63125AB-397A-4D7B-BE90-711978DE05A2";
        
        private readonly MacroGraph _macroGraph;

        public Macro(MacroGraph macroGraph) : base(false)
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
            _macroGraph.ValueOutputs.ForEach(x => AddValueOutput(x.Key, flow => GetOutputAsync(x)));
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