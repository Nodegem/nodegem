using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodegem.Common.Dto;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Data.Dto.GraphDtos;

namespace Nodegem.Services.Data.Repositories
{
    public interface IGraphRepository
    {
        Task<GraphDto> GetGraphAsync(Guid graphId);
        Task<bool> IsListenerGraphAsync(Guid graphId);
        IEnumerable<GraphDto> GetGraphsAssignedToUser(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid graphId);
        GraphDto CreateGraph(CreateGraphDto graph);
        GraphDto UpdateGraph(GraphDto graph);
        void DeleteGraph(Guid graphId);
    }
}