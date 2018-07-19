namespace PasteBin.Data.Repositories
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class EfRepository<TEntity> : IEfRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public EfRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public IQueryable<TEntity> All() => this.dbSet;

        public TEntity Get(object id) => this.dbSet.Find(id);

        public void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public int SaveChanges() => this.context.SaveChanges();

        public void Dispose() => this.context.Dispose();
    }
}
