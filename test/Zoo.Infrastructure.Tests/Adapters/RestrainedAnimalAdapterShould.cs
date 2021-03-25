namespace Zoo.Infrastructure.Tests.Adapters
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Adapters;
    using Infrastructure.Adapters.Specification;
    using Infrastructure.Entities.Zoo;
    using Infrastructure.Store;

    using Moq;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class RestrainedAnimalAdapterShould
    {
        [Test]
        public void ReturnMappedAnimalWhenCallGet()
        {
            var expectedBear = Builder<BearRestrained>.CreateListOfSize(3)
                                                      .Build()
                                                      .AsQueryable();
            var animals = Builder<Animal>.CreateListOfSize(10)
                                         .Build()
                                         .AsQueryable();
            var mockedReader = new Mock<IReader>();
            mockedReader.Setup(reader => reader.Get(It.IsNotNull<AnimalsSpecification<BearRestrained>>()))
                        .Returns(animals);
            var mockedMapper = new Mock<IMapper>();
            mockedMapper.Setup(mapper => mapper.ProjectTo(animals, (object)null, It.IsNotNull<Expression<Func<BearRestrained, object>>[]>()))
                        .Returns(expectedBear);
            var adapter = new RestrainedAnimalAdapter(mockedReader.Object, mockedMapper.Object);

            var actualValue = adapter.Get<BearRestrained>();

            actualValue.Should().BeEquivalentTo(expectedBear);
        }
    }
}