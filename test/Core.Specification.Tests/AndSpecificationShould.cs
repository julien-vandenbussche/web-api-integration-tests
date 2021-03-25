namespace Core.Specification.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Testing;

    [TestFixture]
    public class AndSpecificationShould
    {
        [Test]
        public void ReturnTrueOnAndConditionalSpecificationWhenCallSatisfyAndConditionIsRight()
        {
            const int expectedEntityId = 1;
            const int expectedSubValueId = 3;
            var subEntity = new SubEntity
                                {
                                    EntityId = expectedEntityId,
                                    Values = new List<SubValue>
                                                 {
                                                     new SubValue
                                                         {
                                                             SubValueId = expectedSubValueId
                                                         }
                                                 }
                                };
            var right = new GetSubEntityByParentIdSpecification(expectedEntityId);
            var specification = new GetSubEntityContainsSubValueIdSpecification(expectedSubValueId);

            var conditional = specification.And(right);

            var actualValue = conditional.Satisfy(subEntity);
            
            actualValue.Should().BeTrue();
        }
        
        [Test]
        public void ReturnFalseOnAndConditionalSpecificationWhenCallSatisfyAndConditionIsWrong()
        {
            const int expectedEntityId = 1;
            const int expectedSubValueId = 3;
            const int wrongSubValueId = 45;
            var subEntity = new SubEntity
                                {
                                    EntityId = expectedEntityId,
                                    Values = new List<SubValue>
                                                 {
                                                     new SubValue
                                                         {
                                                             SubValueId = expectedSubValueId
                                                         }
                                                 }
                                };
            var right = new GetSubEntityByParentIdSpecification(expectedEntityId);
            var specification = new GetSubEntityContainsSubValueIdSpecification(wrongSubValueId);

            var conditional = specification.And(right);

            var actualValue = conditional.Satisfy(subEntity);
            
            actualValue.Should().BeFalse();
        }

        [Test]
        public void MergeRelationshipWhenSpecificationIsCombined()
        {
            var right = new GetSubEntityByParentIdSpecification(It.IsAny<int>());
            var specification = new GetSubEntityContainsSubValueIdSpecification(It.IsAny<int>());
            var conditional = specification.And(right);

            conditional.Relationships.Should().BeEquivalentTo(specification.Relationships);
        }
    }
}