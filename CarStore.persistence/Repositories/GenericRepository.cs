using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using CarStore.persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
                                                            where TEntity : BaseEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            =>await _dbSet.ToListAsync();


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
           return await _dbSet.GetQuery<TEntity,TKey>(specification).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification)
        {
            return await _dbSet.GetQuery<TEntity, TKey>(specification).FirstOrDefaultAsync();
        }
    }
}
