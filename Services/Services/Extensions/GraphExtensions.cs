using Microsoft.AspNetCore.DataProtection;
using Nodegem.Common.Dto;
using Nodegem.Data.Models;

namespace Nodegem.Services.Extensions
{
    public static class GraphExtensions
    {

        public static GraphDto EncryptedGraph(this GraphDto graph, IDataProtector protector)
        {
            graph.Constants = graph.Constants.EncryptConstants(protector);
            return graph;
        }
        
        public static GraphDto DecryptGraph(this GraphDto graph, IDataProtector protector)
        {
            graph.Constants = graph.Constants.DecryptConstants(protector);
            return graph;
        }
        
        public static Graph EncryptedGraph(this Graph graph, IDataProtector protector)
        {
            graph.Constants = graph.Constants.EncryptConstants(protector);
            return graph;
        }
        
        public static Graph DecryptGraph(this Graph graph, IDataProtector protector)
        {
            graph.Constants = graph.Constants.DecryptConstants(protector);
            return graph;
        }
        
    }
}