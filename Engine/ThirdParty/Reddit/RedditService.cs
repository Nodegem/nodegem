using Nodester.ThirdParty.Reddit.Exceptions;
using Reddit;
using ThirdParty.Data.Reddit;

namespace Nodester.ThirdParty.Reddit
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