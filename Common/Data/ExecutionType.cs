using System.Runtime.Serialization;

namespace Nodegem.Common.Data
{
    public enum ExecutionType
    {
        [EnumMember(Value = "manual")]
        Manual,
        [EnumMember(Value = "recurring")]
        Recurring,
        [EnumMember(Value = "listener")]
        Listener
    }
}