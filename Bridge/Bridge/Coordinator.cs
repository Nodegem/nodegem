using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Microsoft.Extensions.Logging;
using Nodester.Bridge.Extensions;
using Nodester.Common.Data;
using Nodester.Common.Dto;
using Nodester.Common.Extensions;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Data.Models;
using Nodester.Data.Models.Json_Models;
using Nodester.Engine.Data;
using Nodester.Engine.Data.Exceptions;

namespace Nodester.Bridge
{
    public class Coordinator : IDisposable
    {
        private readonly IBuildGraphService _buildGraphService;
        private readonly IBuildMacroService _buildMacroService;
        private readonly IGraphHubConnection _graphConnection;

        private IDictionary<Guid, IFlowGraph> CompiledRecurringGraphs { get; set; }
        private IDictionary<Guid, IListenerGraph> CompiledListenerGraphs { get; set; }
        private IDictionary<Guid, RecurringGraphState> GraphStates { get; set; }
        private IDictionary<Guid, IListenerGraph> ListenerGraphSandbox { get; }

        private readonly ILogger _logger;

        public Coordinator(IGraphHubConnection graphConnection, IBuildGraphService buildGraphService,
            IBuildMacroService buildMacroService, ILogger logger)
        {
            _buildGraphService = buildGraphService;
            _buildMacroService = buildMacroService;
            _graphConnection = graphConnection;
            _logger = logger;

            ListenerGraphSandbox = new Dictionary<Guid, IListenerGraph>();

            graphConnection.ExecuteGraphEvent += OnRemoteExecuteGraphAsync;
            graphConnection.ExecuteMacroEvent += OnRemoteExecuteMacroAsync;
        }

        public async Task InitializeAsync()
        {
            var compiledRecurringGraphList =
                await AppState.Instance.RecurringGraphs.SelectAsync(async x =>
                    new KeyValuePair<Guid, IFlowGraph>(x.Id,
                        await _buildGraphService.BuildGraphAsync(AppState.Instance.User, x)));

            compiledRecurringGraphList = compiledRecurringGraphList.Where(x => x.Value != null).ToList();

            CompiledRecurringGraphs = compiledRecurringGraphList.ToDictionary(k => k.Key, v => v.Value);

            GraphStates = AppState.Instance.RecurringGraphs.ToDictionary(k => k.Id, v => new RecurringGraphState
            {
                LastRan = v.RecurringOptions.Start
            });

            var compiledListenerGraphList =
                await AppState.Instance.ListenerGraphs.SelectAsync(async x =>
                    new KeyValuePair<Guid, IListenerGraph>(x.Id,
                        await _buildGraphService.BuildGraphAsync(AppState.Instance.User, x) as IListenerGraph));

            CompiledListenerGraphs = compiledListenerGraphList.ToDictionary(k => k.Key, v => v.Value);
        }

        private async Task OnRemoteExecuteGraphAsync(GraphDto graph)
        {
            try
            {
                if (graph.Type == ExecutionType.Listener)
                {
                    await ManageSandboxListenerGraphsAsync(graph);
                }
                else
                {
                    await _buildGraphService.ExecuteFlowGraphAsync(AppState.Instance.User, graph, false);
                }
                
                await _graphConnection.OnGraphCompleteAsync();
            }
            catch (Exception ex)
            {
                await _graphConnection.OnGraphCompleteAsync(new ExecutionErrorData
                {
                    Bridge = AppState.Instance.Info,
                    Message = ex.Message,
                    GraphId = graph.Id.ToString(),
                    GraphName = graph.Name,
                    IsBuildError = ex is GraphBuildException
                });
            }
        }

        private async Task ManageSandboxListenerGraphsAsync(GraphDto graph)
        {
            if (ListenerGraphSandbox.ContainsKey(graph.Id))
            {
                try
                {
                    var listenerGraph = ListenerGraphSandbox[graph.Id];
                    await listenerGraph.DisposeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occured while executing graph {graph.Name}");
                    ListenerGraphSandbox.Remove(graph.Id);
                }
            }

            var compiledListenerGraph = await _buildGraphService.BuildListenerGraphAsync(AppState.Instance.User, graph);
            compiledListenerGraph.IsRunningLocally = false;
            ListenerGraphSandbox[graph.Id] = compiledListenerGraph;

            await compiledListenerGraph.RunAsync();
        }

        private async Task OnRemoteExecuteMacroAsync(MacroDto macro, string flowInputFieldId)
        {
            try
            {
                await _buildMacroService.ExecuteMacroAsync(AppState.Instance.User, macro, flowInputFieldId, false);
                await _graphConnection.OnGraphCompleteAsync();
            }
            catch (Exception ex)
            {
                await _graphConnection.OnGraphCompleteAsync(new ExecutionErrorData
                {
                    Bridge = AppState.Instance.Info,
                    Message = ex.Message,
                    GraphId = macro.Id.ToString(),
                    GraphName = macro.Name,
                    IsBuildError = ex is GraphBuildException
                });
            }
        }

        public async Task ExecuteRecurringGraphsAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await ManageRecurringGraphsAsync();
                await Task.Delay(1, cancelToken);
            }
        }

        private async Task ManageListenerGraphs()
        {
            await Task.CompletedTask;
        }

        private async Task ManageRecurringGraphsAsync()
        {
            var graphLookup = AppState.Instance.GraphLookUp;

            foreach (var (id, flowGraph) in CompiledRecurringGraphs)
            {
                var now = DateTime.UtcNow;
                var graphState = GraphStates[id];
                var options = graphLookup[id].RecurringOptions;
                bool canRun;
                switch (options.Frequency)
                {
                    case FrequencyOptions.Yearly:
                        canRun = now.YearDifference(graphState.LastRan) >= options.Every;
                        break;
                    case FrequencyOptions.Monthly:
                        canRun = now.MonthDifference(graphState.LastRan) >= options.Every;
                        break;
                    case FrequencyOptions.Daily:
                        canRun = (now - graphState.LastRan).TotalDays >= options.Every;
                        break;
                    case FrequencyOptions.Hourly:
                        canRun = (now - graphState.LastRan).TotalHours >= options.Every;
                        break;
                    case FrequencyOptions.Minutely:
                        canRun = (now - graphState.LastRan).TotalMinutes >= options.Every;
                        break;
                    case FrequencyOptions.Secondly:
                        canRun = (now - graphState.LastRan).TotalSeconds >= options.Every;
                        break;
                    default:
                        continue;
                }

                if (!canRun) continue;

                await flowGraph.RunAsync();
                GraphStates[id].LastRan = DateTime.UtcNow;
            }
        }

        public void Dispose()
        {
            AppState.Instance.GraphLookUp.Clear();
        }

        private class RecurringGraphState
        {
            public DateTime LastRan { get; set; }
        }
    }
}