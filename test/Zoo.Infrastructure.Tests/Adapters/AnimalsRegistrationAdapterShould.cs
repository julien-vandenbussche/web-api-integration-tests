namespace Zoo.Infrastructure.Tests.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Administration.AnimalsRegistrationAggregate.Models;

    using AutoMapper;

    using Entities.Zoo;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Adapters;
    using Infrastructure.Adapters.Specification;
    using Infrastructure.Store;

    using Moq;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class AnimalsRegistrationAdapterShould
    {
        [Test]
        public async Task ReturnDetailsOfCreatedAnimalsWhenCallRegister()
        {
            var bearCreating = Builder<BearCreating>.CreateNew().Build();
            var mappedAnimal = Builder<Animal>.CreateNew().Build();
            var animals = Builder<Animal>.CreateListOfSize(10).Build();
            animals.Add(mappedAnimal);
            var queryable = animals.AsQueryable();
            var expectedQuery = queryable.Where(animal => animal == mappedAnimal);
            var expectedBearDetails = Builder<BearDetails>.CreateNew().Build();
            var mockedMapper = new Mock<IMapper>();
            mockedMapper.Setup(mapper => mapper.Map<Animal>(bearCreating))
                        .Returns(mappedAnimal);
            var mockedWriter = new Mock<IWriter>();
            mockedWriter.Setup(writer => writer.Create(mappedAnimal));
            mockedWriter.Setup(writer => writer.SaveAsync(default))
                        .Returns(Task.CompletedTask);
            var mockedReader = new Mock<IReader>();
            mockedReader.Setup(reader => reader.Get(It.IsNotNull<AnimalsSpecification<BearDetails>>()))
                        .Returns(queryable);
            mockedMapper.Setup(
                            mapper => mapper.ProjectTo(
                                expectedQuery,
                                null,
                                It.IsNotNull<Expression<Func<BearDetails, object>>[]>()))
                        .Returns(() => new List<BearDetails> { expectedBearDetails }.AsQueryable());
            var adapter = new AnimalsRegistrationAdapter(mockedMapper.Object, mockedWriter.Object, mockedReader.Object);

            var actualValue = await adapter.RegisterAsync<BearCreating, BearDetails>(bearCreating);

            actualValue.Should().Be(expectedBearDetails);
        }
    }
}