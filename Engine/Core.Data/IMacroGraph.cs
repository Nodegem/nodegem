using System.Collections.Generic;
using Nodester.Graph.Core.Data.Fields;
using Nodester.Graph.Core.Data.Nodes;

namespace Nodester.Graph.Core.Data
{
    public interface IMacroGraph : IGraph
    {
        void Run(string flowInputFieldKey);
        INode ToMacroNode();
        void PopulateInputsWithNewDefaults(IEnumerable<FieldData> fieldData);
        IMacroFlowInputField GetInputByKey(string key);
        IMacroValueOutputField GetOutputByKey(string key);
        void Run(IMacroFlowInputField input);
    }
}