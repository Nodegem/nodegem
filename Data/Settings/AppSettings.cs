namespace Nodegem.Data.Settings
{
    public class AppSettings
    {
        public string AppName { get; set; }
        public string Host { get; set; }
        public string SecretKey { get; set; }
        public bool HostFrontEnd { get; set; }
        public bool IsUsingEmail { get; set; }
    }
}