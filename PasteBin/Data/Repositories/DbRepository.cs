using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PasteBin.Data.Repositories
{
    public class DbRepository<TEntity> : IDbRepository<TEntity> 
        where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public DbRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return this.AddIncludes(this.dbSet).AsQueryable();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> criteria)
        {
            return this.All().Where(criteria).FirstOrDefault();
        }

        public void Add(TEntity entity)
        {
            this.dbSet.Add(entity);

            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.dbSet.Attach(entity);

            this.context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);

            this.context.SaveChanges();
        }

        protected virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable)
        {
            return queryable;
        }
    }
}