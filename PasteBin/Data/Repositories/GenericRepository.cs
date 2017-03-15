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
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> criteria)
        {
            return await GetQuery().Where(criteria).ToListAsync();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> criteria)
        {
            return await GetQuery().Where(criteria).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetQuery().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            await _context.SaveChangesAsync();
        }

        protected virtual IQueryable<T> AddIncludes(IQueryable<T> queryable)
        {
            return queryable;
        }

        protected IQueryable<T> GetQuery()
        {
            return AddIncludes(_dbSet);
        }
    }
}
