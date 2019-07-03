using System.Runtime.Serialization;

namespace Nodester.Engine.Data
{
    public enum GraphType
    {
        [EnumMember(Value = "graph")]
        Graph,
        [EnumMember(Value = "macro")]
        Macro
    }
}