using System;
using System.Collections.Generic;
using Nodester.Data.Models.Json_Models;

namespace Nodester.Data.Models
{
    
    public abstract class BaseGraph : ActiveEntity
    {
        public Guid UserId { get; set; }
        public bool IsDebugModeEnabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Node> Nodes { get; set; }
        public ApplicationUser User { get; set; }
    }
}