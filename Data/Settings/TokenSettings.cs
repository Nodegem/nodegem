namespace Nodegem.Data.Settings
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Expiration { get; set; }
        public long ExpirationBuffer { get; set; }
    }
}