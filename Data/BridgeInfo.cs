using System;

namespace Nodester.Data
{
    public struct BridgeInfo
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
        public string DeviceName { get; set; }
        public int ProcessorCount { get; set; }
        public string OperatingSystem { get; set; }
    }
}