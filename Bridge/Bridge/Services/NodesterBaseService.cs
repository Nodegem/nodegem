using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Nodester.Bridge.Services
{
    public abstract class NodesterBaseService
    {
        protected HttpClient Client { get; }

        protected NodesterBaseService(HttpClient client)
        {
            Client = client;
        }
    }
}