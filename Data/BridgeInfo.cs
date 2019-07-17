using System;

namespace Nodester.Data
{
    public struct BridgeInfo
    {
        public string ConnectionId { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
        public int ProcessorCount { get; set; }
        public string OperatingSystem { get; set; }
    }
}