using Bridge.Data;
using Nodester.Engine.Data.Attributes;
using Nodester.Graph.Core;
using ThirdParty.Data.Reddit;

namespace Nodester.ThirdParty.Reddit.StreamNodes
{
    [NodeNamespace("Third Party.Reddit")]
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