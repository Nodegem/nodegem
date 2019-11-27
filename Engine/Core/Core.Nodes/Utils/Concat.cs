using System.Threading.Tasks;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;

namespace Nodegem.Engine.Core.Nodes.Utils
{
    [DefinedNode("33ABFC2F-BC96-4D36-99D5-86B27B3F5E55")]
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