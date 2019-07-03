using System.Collections.Generic;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
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