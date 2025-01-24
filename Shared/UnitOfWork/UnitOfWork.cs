using Shared.DBContext;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataDBContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed = false;

        public UnitOfWork(DataDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                // Creating a new repository instance for the specific entity type
                var repository = new Repository<TEntity>(_context);
                _repositories[type] = repository;
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Ensure all changes are saved asynchronously
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // Dispose method implementation for releasing DbContext
        public void Dispose()
        {
            if (!_disposed)
            {
                _context.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        // Finalizer to ensure Dispose is called in case it's missed
        ~UnitOfWork()
        {
            Dispose();
        }
    }
    }
