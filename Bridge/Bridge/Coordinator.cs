using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Common.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;

namespace Nodester.Bridge
{
    public class Coordinator
    {

        private readonly IBuildGraphService _buildGraphService;
        
        public Coordinator(IGraphHubConnection graphConnection, IBuildGraphService buildGraphService)
        {
            _buildGraphService = buildGraphService;

            graphConnection.ExecuteGraphEvent += OnExecuteGraph;
            graphConnection.ExecuteMacroEvent += OnExecuteMacro;
        }

        private async Task OnExecuteGraph(GraphDto graph)
        {
            var builtGraph = await _buildGraphService.BuildGraph(graph);
            builtGraph.Run();
        }
        
        private async Task OnExecuteMacro(MacroDto graph, string flowInputFieldId)
        {
            
        }
        
    }
}