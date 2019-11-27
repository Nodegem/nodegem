using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Core;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Integrations.Data.Reddit;

namespace Nodegem.Engine.Integrations.Reddit.StreamNodes
{
    [NodeNamespace("Integrations.Reddit")]
    public abstract class RedditStreamNode : StreamNode
    {
        public IRedditService RedditService { get; set; }

        public RedditStreamNode(IRedditService redditService, IGraphHubConnection graphHubConnection) : base(
            graphHubConnection)
        {
            RedditService = redditService;
        }

    }
}