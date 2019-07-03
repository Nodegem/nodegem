using System;
using System.Threading;
using System.Threading.Tasks;
using Nodester.Common.Data.Interfaces;

namespace Bridge.Data
{
    public interface ITerminalHubConnection : ITerminalHubService, IDisposable
    {
        Task StartAsync(CancellationToken cancelToken);

        Task StopAsync(CancellationToken cancelToken);
    }
}