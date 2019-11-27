namespace Nodegem.Common.Data
{
    public struct ExecutionErrorData
    {
        public string Message { get; set; }
        public string GraphName { get; set; }
        public string GraphId { get; set; }
        public BridgeInfo Bridge { get; set; }
        public bool IsBuildError { get; set; }
    }
}