using System;

namespace Nodegem.Common
{
    public struct BridgeInfo
    {
        public string ConnectionId { get; set; }
        public Guid UserId { get; set; }
        public string DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
        public int ProcessorCount { get; set; }
        public string OperatingSystem { get; set; }
    }
}