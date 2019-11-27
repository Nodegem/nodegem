using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;
using Nodegem.Engine.Data;
using Nodegem.Engine.Data.Attributes;
using Nodegem.Engine.Data.Fields;
using Nodegem.Engine.Integrations.Data.Reddit;

namespace Nodegem.Engine.Integrations.Reddit.StreamNodes
{
    [DefinedNode("7DE6D8DA-9C6F-424B-89B9-882D549B0083")]
    public class StartBot : RedditStreamNode
    {
        
        public IFlowInputField In { get; set; }
        public IValueInputField AppId { get; set; }
        public IValueInputField RefreshToken { get; set; }
        
        public StartBot(IRedditService redditService, IGraphHubConnection graphHubConnection) : base(redditService,
            graphHubConnection)
        {
        }

        protected override void Define()
        {
            base.Define();
            AppId = AddValueInput(nameof(AppId), default(string));
            RefreshToken = AddValueInput(nameof(RefreshToken), default(string));
            In = AddFlowInput(nameof(In), StartConnectionAsync);
        }

        public override void SetupEventListeners()
        {
            //Ignore
        }

        private async Task<IFlowOutputField> StartConnectionAsync(IFlow flow)
        {
            var appId = await flow.GetValueAsync<string>(AppId);
            var refreshToken = await flow.GetValueAsync<string>(RefreshToken);
            RedditService.InitializeClient(appId, refreshToken);
            return Out;
        }
    }
}