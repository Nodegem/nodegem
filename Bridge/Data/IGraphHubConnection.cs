using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge.Data
{
    public interface IGraphHubConnection : IDisposable
    {

        Task StartAsync(CancellationToken cancelToken);

        Task StopAsync(CancellationToken cancelToken);

    }
}