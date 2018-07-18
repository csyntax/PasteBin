namespace PasteBin.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEfRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        TEntity Get(object id);

        Task<TEntity> GetAsync(object id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}