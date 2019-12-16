using System.Collections.Generic;
using System.Linq;

namespace Nodegem.Common.Data
{
    public class ClientData
    {
        public List<BridgeInfo> Bridges { get; set; }
        public List<string> WebClientConnectionIds { get; set; }

        public bool ContainsConnectionId(string connectionId)
        {
            return Bridges.Any(x => x.GraphHubConnectionId == connectionId);
        }
    }
}