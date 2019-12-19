using Reddit;

namespace Nodegem.Engine.Integrations.Data.Reddit
{
    public interface IRedditService
    {
        RedditClient Client { get; }
        
        void InitializeClient(string appId, string refreshToken);
    }
}