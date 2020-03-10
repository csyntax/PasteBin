namespace PasteBin.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PasteBin.Data.Contracts.Repositories;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public EfRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> All() => 
            this.dbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => 
            this.dbSet.AsNoTracking();

        public virtual Task<TEntity> GetByIdAsync(object id) =>
           this.dbSet.FindAsync(id).AsTask();

        public virtual Task AddAsync(TEntity entity) => 
            this.dbSet.AddAsync(entity).AsTask();

        public void Update(TEntity entity)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.dbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.context.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }
    }
}