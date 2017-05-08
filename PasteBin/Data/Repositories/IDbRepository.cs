using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PasteBin.Data.Repositories
{
    public interface IDbRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}