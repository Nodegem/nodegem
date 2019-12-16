using Reddit;

namespace Nodegem.Engine.Integrations.Data.Reddit
{
    public interface IRedditService
    {
        RedditAPI Client { get; }
        
        void InitializeClient(string appId, string refreshToken);
    }
}