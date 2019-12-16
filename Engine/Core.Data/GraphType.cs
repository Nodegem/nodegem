using System.Runtime.Serialization;

namespace Nodegem.Engine.Data
{
    public enum GraphType
    {
        [EnumMember(Value = "graph")]
        Graph,
        [EnumMember(Value = "macro")]
        Macro
    }
}