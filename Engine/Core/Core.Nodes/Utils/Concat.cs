using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.Utils
{
    [DefinedNode]
    [NodeNamespace("Core.Utils")]
    public class Concat : Node
    {
        public IValueInputField A { get; set; }
        public IValueInputField B { get; set; }
        public IValueOutputField String { get; set; }
        
        protected override void Define()
        {
            A = AddValueInput<string>(nameof(A));
            B = AddValueInput<string>(nameof(B));
            String = AddValueOutput(nameof(String), ConcatString);
        }

        private async Task<string> ConcatString(IFlow flow)
        {
            var a = await flow.GetValueAsync<string>(A);
            var b = await flow.GetValueAsync<string>(B);
            return a + b;
        }
    }
}