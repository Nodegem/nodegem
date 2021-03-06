using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nodegem.Data.Contexts;
using Nodegem.Data.Extensions;
using Nodegem.Data.Models;
using Nodegem.Services.Data.Repositories;

namespace Nodegem.Services.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly NodegemContext Context;

        protected DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public Repository(NodegemContext context)
        {
            Context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }
        
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public TEntity Get(Guid id)
        {
            return DbSet.Single(x => x.Id == id);
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            return DbSet.SingleAsync(x => x.Id == id);
        }

        public void Create(TEntity entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastUpdated = DateTime.UtcNow;
            DbSet.Add(entity);
            Context.SaveChanges();
        }
        
        public async Task CreateAsync(TEntity entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastUpdated = DateTime.UtcNow;
            await DbSet.AddAsync(entity);
        }

        public void Update(Guid id, TEntity entity)
        {
            entity.Id = id;
            entity.LastUpdated = DateTime.UtcNow;
            DbSet.Update(entity);
            Context.SaveChanges();
        }
        
        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            DbSet.Remove(x => x.Id == id);
            Context.SaveChanges();
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}