using System.Threading.Tasks;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Engine.Data.Fields;

namespace Nodester.Graph.Core.Nodes.SQL
{
    [DefinedNode]
    [NodeNamespace("Core.SQL")]
    public abstract class SQLNode : Node
    {
        public IValueInputField ConnectionString { get; set; }
        public IFlowInputField In { get; set; }
        public IFlowOutputField Out { get; set; }

        protected override void Define()
        {
            ConnectionString = AddValueInput<string>(nameof(ConnectionString));
            In = AddFlowInput(nameof(In), ExecuteSQLAsync);
            Out = AddFlowOutput(nameof(Out));
        }

        protected abstract Task<IFlowOutputField> ExecuteSQLAsync(IFlow flow);

    }
}