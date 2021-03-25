namespace Core.Specification.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

    using Testing;

    [TestFixture]
    public class NotSpecificationShould
    {
        [Test]
        public void ReturnInverseOfCurrentSpecificationWhenCallSatisfy()
        {
            var entity = new Entity { Id = 1 };
            var currentSpecification = new GetEntityByIdSpecification(1);
            var notSpecification = currentSpecification.Not();

            var actualValue = notSpecification.Satisfy(entity);

            actualValue.Should().Be(!currentSpecification.Satisfy(entity));
        }

        [Test]
        public void MergeRelationshipsWithOriginalSpecification()
        {
            var currentSpecification = new GetEntityByIdSpecification(1);
            var notSpecification = currentSpecification.Not();

            var actualValue = notSpecification.Relationships;

            actualValue.Should().BeEquivalentTo(currentSpecification.Relationships);
        }
    }
}