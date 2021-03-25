namespace Core.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    public delegate void AddRelationship<TEntity>(Relationship<TEntity> include);

    public interface ISpecification<TEntity>
    {
        public static readonly ISpecification<TEntity> All = new IdentitySpecification<TEntity>();

        IImmutableList<Relationship<TEntity>> Relationships { get; }

        ISpecification<TEntity> And(ISpecification<TEntity> specification);

        ISpecification<TEntity> Not();

        ISpecification<TEntity> Or(ISpecification<TEntity> specification);

        bool Satisfy(TEntity entity);

        Expression<Func<TEntity, bool>> ToExpression();
    }

    [ExcludeFromCodeCoverage]
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        private readonly List<Relationship<TEntity>> relationships = new List<Relationship<TEntity>>();

        public IImmutableList<Relationship<TEntity>> Relationships
        {
            get
            {
                if (!this.relationships.Any())
                {
                    this.OnAddRelation(this.relationships.Add);
                }

                return this.relationships.ToImmutableList();
            }
        }

        public ISpecification<TEntity> And(ISpecification<TEntity> specification)
        {
            if (this == ISpecification<TEntity>.All)
            {
                return specification;
            }

            if (specification == ISpecification<TEntity>.All)
            {
                return this;
            }

            return new ConditionSpecification<TEntity>(this, specification, Expression.AndAlso);
        }

        public ISpecification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }

        public ISpecification<TEntity> Or(ISpecification<TEntity> specification)
        {
            if (this == ISpecification<TEntity>.All || specification == ISpecification<TEntity>.All)
            {
                return ISpecification<TEntity>.All;
            }

            return new ConditionSpecification<TEntity>(this, specification, Expression.OrElse);
        }

        public bool Satisfy(TEntity entity)
        {
            var predicate = this.ToExpression().Compile();
            return predicate(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        protected virtual void OnAddRelation(AddRelationship<TEntity> addRelationship)
        {
        }
    }
}