using System;

namespace Nodester.Graph.Core.Data.Fields
{
    public interface IFlowInputField : IFlowField
    {
        Func<IFlow, IFlowOutputField> Action { get; }
    }
}