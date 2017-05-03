using System;
using System.Linq;

namespace PasteBin.Data.Repositories
{
    public interface IDbRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        TEntity Find(int id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int SaveChanges();
    }
}