using NUnit.Framework;

namespace Zoo.Infrastructure.Tests.Adapters.Specification
{
    using Administration.AnimalsRegistrationAggregate.Models;

    using FluentAssertions;

    using Infrastructure.Adapters.Specification;
    using Infrastructure.Entities.Zoo;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class AnimalsSpecificationShould
    {
        [Test]
        public void ReturnTrueWhenBearAnimalHaveFamilyIdEqualTo3()
        {
            var specification = new AnimalsSpecification<BearDetails>();
            var animal = new Animal
                             {
                                 FamilyId = 3
                             };
            var satisfy = specification.Satisfy(animal);

            satisfy.Should().BeTrue();
        }

        [Test]
        public void ReturnFalseWhenBearAnimalHaveFamilyIdNotEqualTo3()
        {
            var specification = new AnimalsSpecification<BearRestrained>();
            var animal = new Animal
                             {
                                 FamilyId = 4
                             };
            var satisfy = specification.Satisfy(animal);

            satisfy.Should().BeFalse();
        }

        [Test]
        public void HaveAccidentOriginsRelationship()
        {
            var specification = new AnimalsSpecification<BearCreating>();

            specification.Relationships[0].Root.ToString().Should().Be("animal => animal.Family");
        }
    }
}