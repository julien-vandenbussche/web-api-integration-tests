namespace Zoo.Infrastructure.Tests.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using MockQueryable.Moq;

    using Moq;
    using Moq.Language.Flow;

    internal static class MoqExtensions
    {
        public static IReturnsResult<TContext> ReturnsDbSet
            <TContext, TEntity, TPrimaryKey>(
                this ISetup<TContext, DbSet<TEntity>> setup,
                IList<TEntity> entities,
                Func<TEntity, TPrimaryKey> primaryKey)
            where TEntity : class where TContext : class
        {
            var dbSet = AsMockedDbSet(entities);

            if (primaryKey != null)
            {
                dbSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns(
                    (object[] input) =>
                        entities.SingleOrDefault(x => primaryKey(x).Equals((TPrimaryKey)input.First())));
            }

            return setup.Returns(dbSet.Object);
        }

        public static IReturnsResult<TContext> ReturnsDbSet
            <TContext, TEntity>(this ISetup<TContext, DbSet<TEntity>> setup, IList<TEntity> entities)
            where TEntity : class where TContext : class
        {
            var dbSet = AsMockedDbSet(entities);

            return setup.Returns(dbSet.Object);
        }

        private static Mock<DbSet<TEntity>> AsMockedDbSet<TEntity>(IList<TEntity> entities) where TEntity : class
        {
            var queryable = entities.AsQueryable();
            var dbSet = queryable.BuildMockDbSet();
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>(entities.Add);
            return dbSet;
        }
    }
}