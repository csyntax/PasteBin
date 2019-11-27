namespace PasteBin.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEfRepository<TEntity> : IDisposable
        where TEntity : class
    {
        bool Any { get; }

        IQueryable<TEntity> All();

        TEntity Get(object id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}