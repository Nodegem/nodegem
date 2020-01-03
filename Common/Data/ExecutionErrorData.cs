namespace Nodegem.Common.Data
{
    public struct ExecutionErrorData
    {
        public string Message { get; set; }
        public string GraphName { get; set; }
        public NodeErrorData NodeError { get; set; }
        public string GraphId { get; set; }
        public BridgeInfo Bridge { get; set; }
        public bool IsBuildError { get; set; }
    }

    public struct NodeErrorData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}