using System;
using System.Threading.Tasks;

namespace Nodester.Engine.Data.Fields
{
    public interface IFlowInputField : IFlowField
    {
        Func<IFlow, Task<IFlowOutputField>> Action { get; }
    }
}