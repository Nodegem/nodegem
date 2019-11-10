using System.Collections.Generic;
using System.Runtime.Serialization;
using Nodester.Common.Data;
using Nodester.Data.Models.Json_Models;
using Constant = Nodester.Data.Models.Json_Models.Graph_Constants.Constant;

namespace Nodester.Data.Models
{

    public class Graph : BaseGraph
    {
        
        public ExecutionType Type { get; set; }
        
        public RecurringOptions RecurringOptions { get; set; }
        
        public IEnumerable<Link> Links { get; set; }
        
        public IEnumerable<Constant> Constants { get; set; }
    }
}