using System;
using System.Collections.Generic;
using Nodegem.Data.Models.Json_Models;

namespace Nodegem.Data.Models
{
    
    public abstract class BaseGraph : ActiveEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Node> Nodes { get; set; }
        public ApplicationUser User { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}