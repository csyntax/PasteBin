namespace PasteBin.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public sealed class EfRepository<TEntity> : IEfRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public EfRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> All() => this.dbSet.AsQueryable<TEntity>();

        public TEntity Get(object id) => this.dbSet.Find(id);

        public Task<TEntity> GetAsync(object id) => this.dbSet.FindAsync(id);

        public void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = this.dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public int SaveChanges() => this.dbContext.SaveChanges();

        public Task<int> SaveChangesAsync() => this.dbContext.SaveChangesAsync();

        public void Dispose() => this.dbContext.Dispose();
    }
}