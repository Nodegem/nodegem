using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Nodegem.Data.Models;

namespace Nodegem.Services.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        TEntity Get(Guid id);
        Task<TEntity> GetAsync(Guid id);
        void Create(TEntity entity);
        Task CreateAsync(TEntity entity);
        void Update(Guid id, TEntity entity);
        void Delete(TEntity entity);
        void Delete(Guid id);
        int Save();
        Task<int> SaveAsync();
    }
}