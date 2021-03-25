namespace Core.Specification.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

    using Testing;

    [TestFixture]
    public class IdentitySpecificationShould
    {
        [SetUp]
        public void BeforeEach()
        {
        }

        [Test]
        public void ReturnTrueWhenCallSatisfyWithWhateverValue()
        {
            var specification = ISpecification<Entity>.All;

            var actualValue = specification.Satisfy(null);

            actualValue.Should().BeTrue();
        }
        
        [Test]
        public void ReturnEmptyRelationshipsWhenCallRelationships()
        {
            var specification = ISpecification<Entity>.All;

            var actualValue = specification.Relationships;

            actualValue.Should().BeEmpty();
        }
    }
}
