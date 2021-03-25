namespace Core.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq.Expressions;

    public sealed class Relationship<TEntity>
    {
        private readonly List<Expression<Func<object, object>>> children = new List<Expression<Func<object, object>>>();

        private Relationship(Expression<Func<TEntity, object>> root)
        {
            this.Root = root;
        }

        public IImmutableList<Expression<Func<object, object>>> Children => this.children.ToImmutableList();

        public Expression<Func<TEntity, object>> Root { get; }

        public static Relationship<TEntity> Create(Expression<Func<TEntity, object>> root)
        {
            var relationship = new Relationship<TEntity>(root);
            return relationship;
        }

        public Relationship<TEntity> Then<TProperty>(Expression<Func<TProperty, object>> navigationPropertyPath)
        {
            var expression = Convert(navigationPropertyPath);
            this.children.Add(expression);
            return this;
        }

        private static Expression<Func<object, object>> Convert
            <TProperty>(Expression<Func<TProperty, object>> expression)
        {
            var parameter = Expression.Parameter(typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(
                Expression.Invoke(expression, Expression.Convert(parameter, typeof(TProperty))),
                parameter);
            return lambda;
        }
    }
}