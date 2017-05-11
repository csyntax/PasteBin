using System;
using System.Linq;
using System.Linq.Expressions;

namespace PasteBin.Data.Repositories
{
    public interface IDbRepository<TEntity> 
        where TEntity : class
    {
        IQueryable<TEntity> All();

        TEntity Find(Expression<Func<TEntity, bool>> criteria);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}