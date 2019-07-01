using System.Runtime.Serialization;

namespace Nodester.Graph.Core.Data
{
    public enum GraphType
    {
        [EnumMember(Value = "graph")]
        Graph,
        [EnumMember(Value = "macro")]
        Macro
    }
}