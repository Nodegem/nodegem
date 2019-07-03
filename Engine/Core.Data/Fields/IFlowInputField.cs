using System;

namespace Nodester.Engine.Data.Fields
{
    public interface IFlowInputField : IFlowField
    {
        Func<IFlow, IFlowOutputField> Action { get; }
    }
}