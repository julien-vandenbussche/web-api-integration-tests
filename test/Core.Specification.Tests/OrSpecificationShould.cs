namespace Core.Specification.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    using Testing;

    [TestFixture]
    public class OrSpecificationShould
    {
        [Test]
        public void ReturnTrueOnOrConditionalSpecificationWhenCallSatisfyOrConditionTheFirstConditionIsRight()
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

            var conditional = specification.Or(right);

            var actualValue = conditional.Satisfy(subEntity);
            
            actualValue.Should().BeTrue();
        }
        
        [Test]
        public void ReturnTrueOnOrConditionalSpecificationWhenCallSatisfyOrConditionTheSecondConditionIsRight()
        {
            const int expectedEntityId = 1;
            const int expectedSubValueId = 3;
            const int wrongEntityId = 45;
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
            var right = new GetSubEntityByParentIdSpecification(wrongEntityId);
            var specification = new GetSubEntityContainsSubValueIdSpecification(expectedSubValueId);

            var conditional = specification.Or(right);

            var actualValue = conditional.Satisfy(subEntity);
            
            actualValue.Should().BeTrue();
        }
        
        [Test]
        public void ReturnFalseOnOrConditionalSpecificationWhenCallSatisfyOrConditionAllConditionIsWrong()
        {
            const int expectedEntityId = 1;
            const int expectedSubValueId = 3;
            const int wrongEntityId = 45;
            const int wrongSubValueId = 33;
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
            var right = new GetSubEntityByParentIdSpecification(wrongEntityId);
            var specification = new GetSubEntityContainsSubValueIdSpecification(wrongSubValueId);

            var conditional = specification.Or(right);

            var actualValue = conditional.Satisfy(subEntity);
            
            actualValue.Should().BeFalse();
        }
    }
}