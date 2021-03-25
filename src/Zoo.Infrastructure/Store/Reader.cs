namespace Zoo.Infrastructure.Store
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.Specification;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;

    internal interface IReader : IDisposable
    {
        IQueryable<TEntity> Get<TEntity>(ISpecification<TEntity> specification) where TEntity : class;
    }

    internal sealed class Reader : IReader
    {
        private IDbContext context;

        private bool disposed;

        public Reader(IDbContext context)
        {
            this.context = context;
        }

        ~Reader()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> Get<TEntity>(ISpecification<TEntity> specification) where TEntity : class
        {
            return Get(this.context.Set<TEntity>(), Queryable.Where, specification);
        }

        private static IQueryable<T> Get<T>(
            IQueryable<T> query,
            Func<IQueryable<T>, Expression<Func<T, bool>>, IQueryable<T>> func,
            ISpecification<T> specification)
            where T : class
        {
            foreach (var relationship in specification.Relationships)
            {
                query = query.Include(relationship.Root);
                var relationships = (IIncludableQueryable<T, object>)query;
                foreach (var then in relationship.Children)
                {
                    query = relationships.ThenInclude(then);
                }
            }

            query = func(query, specification.ToExpression());
            return query.AsNoTracking();
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