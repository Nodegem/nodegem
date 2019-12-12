using System.Collections.Generic;
using Nodegem.Common.Data;
using Nodegem.Data.Models.Json_Models;

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