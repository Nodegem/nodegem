using System.Collections.Generic;
using Nodegem.Common.Data;
using Nodegem.Data.Models.Json_Models;
using Constant = Nodegem.Data.Models.Json_Models.Graph_Constants.Constant;

namespace Nodegem.Data.Models
{

    public class Graph : BaseGraph
    {
        
        public ExecutionType Type { get; set; }
        
        public RecurringOptions RecurringOptions { get; set; }
        
        public IEnumerable<Link> Links { get; set; }
        
        public IEnumerable<Constant> Constants { get; set; }
    }
}