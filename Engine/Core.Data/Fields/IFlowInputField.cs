using System;
using System.Threading.Tasks;

namespace Nodegem.Engine.Data.Fields
{
    public interface IFlowInputField : IFlowField
    {
        Func<IFlow, Task<IFlowOutputField>> Action { get; }
    }
}