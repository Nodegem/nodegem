using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
{
    public interface IMacroGraph : IGraph
    {
        Task RunAsync(string flowInputFieldKey, bool isLocal = false);
        INode ToMacroNode();
        void PopulateInputsWithNewDefaults(IEnumerable<FieldData> fieldData);
        IMacroFlowInputField GetInputByKey(string key);
        IMacroValueOutputField GetOutputByKey(string key);
        Task RunAsync(IMacroFlowInputField input, bool isLocal = false);
    }
}