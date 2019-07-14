using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Engine.Data.Fields;
using Nodester.Engine.Data.Nodes;

namespace Nodester.Engine.Data
{
    public interface IMacroGraph : IGraph
    {
        Task RunAsync(string flowInputFieldKey);
        Task RunAsync(IMacroFlowInputField input);
        INode ToMacroNode();
        void PopulateInputsWithNewDefaults(IEnumerable<FieldData> fieldData);
        IMacroFlowInputField GetInputByKey(string key);
        IMacroValueOutputField GetOutputByKey(string key);
    }
}