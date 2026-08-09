using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using CarStore.persistence.IdentityData.Data.DbContexts;

namespace CarStore.persistence.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private readonly IdentityAppDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];

        public IdentityUnitOfWork(IdentityAppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var KeyOfDictionary = $" {typeof(TEntity).Name}+{typeof(TKey).Name}";

            if (_repositories.TryGetValue(KeyOfDictionary, out object? repo))
                return (IGenericRepository<TEntity, TKey>)repo;

            var newRepo = new GenericRepository<TEntity, TKey>(_context);
            _repositories.Add(KeyOfDictionary, newRepo);
            return newRepo;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}