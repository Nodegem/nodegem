using System;
using System.Threading;
using System.Threading.Tasks;
using Nodegem.Common.Data.Interfaces;

namespace Nodegem.ClientService.Data
{
    public interface ITerminalHubConnection : ITerminalHubService, IDisposable
    {
        Task StartAsync(CancellationToken cancelToken);

        Task StopAsync(CancellationToken cancelToken);
    }
}