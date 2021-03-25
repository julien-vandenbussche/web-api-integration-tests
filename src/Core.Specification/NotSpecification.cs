namespace Core.Specification
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal sealed class NotSpecification<TEntity> : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> specification;

        public NotSpecification(ISpecification<TEntity> specification)
        {
            this.specification = specification;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var expression = this.specification.ToExpression();
            var notExpression = Expression.Not(expression.Body);

            return Expression.Lambda<Func<TEntity, bool>>(notExpression, expression.Parameters.Single());
        }

        protected override void OnAddRelation(AddRelationship<TEntity> addRelationship)
        {
            foreach (var relationship in this.specification.Relationships)
            {
                addRelationship(relationship);
            }
        }
    }
}