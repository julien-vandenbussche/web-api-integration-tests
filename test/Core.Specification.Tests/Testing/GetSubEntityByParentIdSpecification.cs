namespace Core.Specification.Tests.Testing
{
    using System;
    using System.Linq.Expressions;

    public sealed class GetSubEntityByParentIdSpecification : Specification<SubEntity>
    {
        private readonly int entityId;

        public GetSubEntityByParentIdSpecification(int entityId)
        {
            this.entityId = entityId;
        }

        public override Expression<Func<SubEntity, bool>> ToExpression() => subEntity => subEntity.EntityId == this.entityId;
    }
}