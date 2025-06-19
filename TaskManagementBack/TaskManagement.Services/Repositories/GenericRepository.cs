
using Microsoft.EntityFrameworkCore;
using TaskManagement.Services.Interfaces;


namespace TaskManagement.Services.Repositories
{
    public class GenericRepository<TEntity> : ICrud<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(dbSet.AsQueryable().ToList());
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsQueryable().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Create(entity);
            }
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }

        public TEntity FindBy(Func<TEntity, bool> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public async Task DeleteAsync(params object[] id)
        {
            TEntity entity = await this.FindAsync(id);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public bool Exists(Func<TEntity, bool> predicate)
        {
            return dbSet.Any(predicate);
        }

        public int Count()
        {
            return dbSet.Count();
        }
    }
}
