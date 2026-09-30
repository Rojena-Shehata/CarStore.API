using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarStore.persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
                                                            
    {
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(DbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey Id)
            => await _dbSet.FindAsync(Id);

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await _dbSet.GetQuery<TEntity>(specification).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification)
        {
            return await _dbSet.GetQuery<TEntity>(specification).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await _dbSet.GetQuery<TEntity>(specification).CountAsync();
        }

        public IQueryable<TEntity> GetTable(ISpecification<TEntity> specification)
        {
            return _dbSet.GetQuery<TEntity>(specification);
        }

        public IQueryable<TEntity> GetTable()
        {
            return _dbSet;
        }
    }
}