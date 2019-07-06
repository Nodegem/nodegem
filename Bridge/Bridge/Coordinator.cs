using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Data;
using Nodester.Bridge.Extensions;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Dto.MacroDtos;
using Nodester.Data.Models.Json_Models;
using Nodester.Engine.Data;

namespace Nodester.Bridge
{
    
    public class Coordinator : IDisposable
    {
        private readonly IBuildGraphService _buildGraphService;
        private readonly IBuildMacroService _buildMacroService;

        private IDictionary<Guid, IFlowGraph> CompiledRecurringGraphs { get; set; }
        private IDictionary<Guid, IFlowGraph> CompiledListenerGraphs { get; set; }
        
        private IDictionary<Guid, RecurringGraphState> GraphStates { get; set; }

        public Coordinator(IGraphHubConnection graphConnection, IBuildGraphService buildGraphService,
            IBuildMacroService buildMacroService)
        {
            _buildGraphService = buildGraphService;
            _buildMacroService = buildMacroService;

            graphConnection.ExecuteGraphEvent += OnRemoteExecuteGraphAsync;
            graphConnection.ExecuteMacroEvent += OnRemoteExecuteMacroAsync;
            
        }

        public async Task InitializeAsync()
        {
            var compiledRecurringGraphList =
                await Task.WhenAll(AppState.Instance.RecurringGraphs.Select(async x =>
                    new KeyValuePair<Guid, IFlowGraph>(x.Id,
                        await _buildGraphService.BuildGraphAsync(AppState.Instance.User, x))));

            CompiledRecurringGraphs = compiledRecurringGraphList.ToDictionary(k => k.Key, v => v.Value);

            GraphStates = AppState.Instance.RecurringGraphs.ToDictionary(k => k.Id, v => new RecurringGraphState
            {
                LastRan = v.RecurringOptions.Start 
            });

            var compiledListenerGraphList =
                await Task.WhenAll(AppState.Instance.ListenerGraphs.Select(async x =>
                    new KeyValuePair<Guid, IFlowGraph>(x.Id,
                        await _buildGraphService.BuildGraphAsync(AppState.Instance.User, x))));

            CompiledListenerGraphs = compiledListenerGraphList.ToDictionary(k => k.Key, v => v.Value);
        }

        private async Task OnRemoteExecuteGraphAsync(GraphDto graph)
        {
            await _buildGraphService.ExecuteGraphAsync(AppState.Instance.User, graph);
        }

        private async Task OnRemoteExecuteMacroAsync(MacroDto macro, string flowInputFieldId)
        {
            await _buildMacroService.ExecuteMacroAsync(AppState.Instance.User, macro, flowInputFieldId);
        }

        public async Task ManageGraphsAsync(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                await ManageRecurringGraphs();
                await Task.Delay(100, cancelToken);
            }
        }

        private async Task ManageListenerGraphs()
        {
            await Task.CompletedTask;
        }

        private async Task ManageRecurringGraphs()
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
                    
                await Task.Run(() => flowGraph.Run(true));
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