using System.Collections.Generic;
using System.Runtime.Serialization;
using Nodester.Data.Models.Json_Models;
using Nodester.Data.Models.Json_Models.Graph_Constants;

namespace Nodester.Data.Models
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
    
    public class Graph : BaseGraph
    {
        
        public ExecutionType Type { get; set; }
        
        public RecurringOptions RecurringOptions { get; set; }
        
        public IEnumerable<Link> Links { get; set; }
        
        public IEnumerable<Constant> Constants { get; set; }
    }
}