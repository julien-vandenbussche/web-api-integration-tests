namespace Core.Specification.Tests.Testing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class GetSubEntityContainsSubValueIdSpecification : Specification<SubEntity>
    {
        private readonly int subValueId;

        public GetSubEntityContainsSubValueIdSpecification(int subValueId)
        {
            this.subValueId = subValueId;
        }

        public override Expression<Func<SubEntity, bool>> ToExpression() => entity => entity.Values.Any(v => v.SubValueId == this.subValueId);
        
        protected override void OnAddRelation(AddRelationship<SubEntity> addRelationship)
        {
            base.OnAddRelation(addRelationship);
            var relationship = Relationship<SubEntity>.Create(entity => entity.Values);
            addRelationship(relationship);
        }
    }
}