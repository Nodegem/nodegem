using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Fields;
using ThirdParty.Data.Reddit;

namespace Nodester.ThirdParty.Reddit.StreamNodes
{
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