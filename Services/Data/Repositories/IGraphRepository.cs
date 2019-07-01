using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.GraphDtos;

namespace Nodester.Services.Data.Repositories
{
    public interface IGraphRepository
    {
        Task<GraphDto> GetGraphAsync(Guid graphId);
        IEnumerable<GraphDto> GetAllGraphsByUser(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid graphId);
        GraphDto CreateGraph(CreateGraphDto graph);
        GraphDto UpdateGraph(GraphDto graph);
        void DeleteGraph(Guid graphId);
    }
}