namespace PasteBin.Data.Repositories
{
    using System;
    using System.Linq;

    public interface IEfRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        TEntity Get(object id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int SaveChanges();
    }
}