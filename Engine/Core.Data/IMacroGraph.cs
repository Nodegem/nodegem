using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Data.Nodes;

namespace Nodegem.Engine.Data
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