using Microsoft.EntityFrameworkCore;
using PasteBin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PasteBin.Data.Repositories
{
    public class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public DbRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            this.dbSet.Add(entity);

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await this.GetQuery().Where(criteria).ToListAsync();
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await this.GetQuery().Where(criteria).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.GetQuery().ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this.dbSet.Attach(entity);

            await this.context.SaveChangesAsync();
        }

        protected virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable)
        {
            return queryable;
        }

        protected IQueryable<TEntity> GetQuery()
        {
            return this.AddIncludes(this.dbSet);
        }
    }
}