using Nodegem.Engine.Integrations.Data.Reddit;
using Nodegem.Engine.Integrations.Reddit.Exceptions;
using Reddit;

namespace Nodegem.Engine.Integrations.Reddit
{
    public class RedditService : IRedditService
    {
        public RedditAPI Client { get; private set; }
        
        public void InitializeClient(string appId, string refreshToken)
        {
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(refreshToken))
            {
                throw new RedditException();
            }

            Client = new RedditAPI(appId, refreshToken);
        }
    }
}