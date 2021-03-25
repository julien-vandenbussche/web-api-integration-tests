namespace Zoo.Infrastructure.Store
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IWriter : IDisposable
    {
        void Create<TEntity>(TEntity entity) where TEntity : class;

        Task SaveAsync(CancellationToken cancellationToken = default);

        void Update<TId, TEntity>(TId id, Action<TEntity> update) where TEntity : class;
    }

    internal sealed class Writer : IWriter
    {
        private IDbContext context;

        private bool disposed;

        public Writer(IDbContext context)
        {
            this.context = context;
        }

        ~Writer()
        {
            this.Dispose(false);
        }

        public void Create<T>(T entity) where T : class
        {
            var dbSet = this.context.Set<T>();
            dbSet.Add(entity);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return this.context.SaveChangesAsync(cancellationToken);
        }

        public void Update<TId, TEntity>(TId id, Action<TEntity> update) where TEntity : class
        {
            var dbSet = this.context.Set<TEntity>();
            var existingEntity = dbSet.Find(id);
            update(existingEntity);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing && this.context != null)
            {
                this.context.Dispose();
                this.context = null;
            }

            this.disposed = true;
        }
    }
}