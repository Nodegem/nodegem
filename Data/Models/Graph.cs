using System.Collections.Generic;
using Nodester.Data.Models.Json_Models;
using Nodester.Data.Models.Json_Models.Graph_Constants;

namespace Nodester.Data.Models
{
    public class Graph : BaseGraph
    {
        public IEnumerable<Link> Links { get; set; }
        
        public IEnumerable<Constant> Constants { get; set; }
    }
}