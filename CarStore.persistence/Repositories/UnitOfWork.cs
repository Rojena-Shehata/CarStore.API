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
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            //KeyOfDictionary = "TEntity+TKey" =>"Brand+int"
            var KeyOfDictionary =$" {typeof(TEntity).Name}+{typeof(TKey).Name}";

            if (_repositories.TryGetValue(KeyOfDictionary, out object? repo))
                return (IGenericRepository<TEntity, TKey>)repo;

            
            var newRepo= new GenericRepository<TEntity, TKey>(_context);
            _repositories.Add(KeyOfDictionary, newRepo);
            return newRepo;

        }

        public async Task<int> SaveChangesAsync()
            =>await  _context.SaveChangesAsync();
        
    }
}
