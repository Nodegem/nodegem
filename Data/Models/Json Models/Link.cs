using System;

namespace Nodegem.Data.Models.Json_Models
{
    public class Link
    {
        public Guid SourceNode { get; set; }
        public string SourceKey { get; set; }
        public Guid DestinationNode { get; set; }
        public string DestinationKey { get; set; }
    }
}