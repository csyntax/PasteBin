using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PasteBin.Data.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> 
        where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            this.dbSet.Add(entity);

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria)
        {
            return await this.GetQuery().Where(criteria).ToListAsync();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> criteria)
        {
            return await this.GetQuery().Where(criteria).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.GetQuery().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            this.dbSet.Attach(entity);

            await this.context.SaveChangesAsync();
        }

        protected virtual IQueryable<T> AddIncludes(IQueryable<T> queryable)
        {
            return queryable;
        }

        protected IQueryable<T> GetQuery()
        {
            return this.AddIncludes(this.dbSet);
        }
    }
}
