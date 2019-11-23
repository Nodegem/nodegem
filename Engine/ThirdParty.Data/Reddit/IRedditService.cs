using Reddit;

namespace ThirdParty.Data.Reddit
{
    public interface IRedditService
    {
        RedditAPI Client { get; }
        
        void InitializeClient(string appId, string refreshToken);
    }
}