using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Bridge
{
    public class Coordinator
    {
        private readonly IBuildGraphService _buildGraphService;
        private readonly IBuildMacroService _buildMacroService;

        public Coordinator(IGraphHubConnection graphConnection, IBuildGraphService buildGraphService,
            IBuildMacroService buildMacroService)
        {
            _buildGraphService = buildGraphService;
            _buildMacroService = buildMacroService;

            graphConnection.ExecuteGraphEvent += OnExecuteGraph;
            graphConnection.ExecuteMacroEvent += OnExecuteMacro;
        }

        private async Task OnExecuteGraph(GraphDto graph)
        {
            await _buildGraphService.ExecuteGraphAsync(AppState.Instance.User, graph);
        }

        private async Task OnExecuteMacro(MacroDto macro, string flowInputFieldId)
        {
            await _buildMacroService.ExecuteMacroAsync(AppState.Instance.User, macro, flowInputFieldId);
        }
    }
}