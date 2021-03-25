namespace Core.Specification
{
    using System;
    using System.Linq.Expressions;

    internal sealed class ConditionSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Func<Expression, Expression, Expression> conditionalFunc;

        private readonly ISpecification<TEntity> left;

        private readonly ISpecification<TEntity> right;

        public ConditionSpecification(
            ISpecification<TEntity> left,
            ISpecification<TEntity> right,
            Func<Expression, Expression, Expression> conditionalFunc)
        {
            this.left = left;
            this.right = right;
            this.conditionalFunc = conditionalFunc;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var leftExpression = this.left.ToExpression();
            var rightExpression = this.right.ToExpression();

            var expression =
                new SwapVisitor(leftExpression.Parameters[0], rightExpression.Parameters[0]).Visit(leftExpression.Body);
            var conditionExpression = this.conditionalFunc(expression, rightExpression.Body);
            return Expression.Lambda<Func<TEntity, bool>>(conditionExpression, rightExpression.Parameters);
        }

        protected override void OnAddRelation(AddRelationship<TEntity> addRelationship)
        {
            foreach (var leftRelationship in this.left.Relationships)
            {
                addRelationship(leftRelationship);
            }
        }

        private class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression from;

            private readonly Expression to;

            public SwapVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == this.from ? this.to : base.Visit(node);
            }
        }
    }
}