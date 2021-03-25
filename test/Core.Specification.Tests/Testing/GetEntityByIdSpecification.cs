namespace Core.Specification.Tests.Testing
{
    using System;
    using System.Linq.Expressions;

    public sealed class GetEntityByIdSpecification : Specification<Entity>
    {
        private readonly int id;

        public GetEntityByIdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Entity, bool>> ToExpression() => entity => entity.Id == this.id;

        protected override void OnAddRelation(AddRelationship<Entity> addRelationship)
        {
            base.OnAddRelation(addRelationship);
            var relationship = Relationship<Entity>.Create(entity => entity.SubEntities);
            addRelationship(relationship);
        }
    }
}