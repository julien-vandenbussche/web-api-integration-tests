namespace Zoo.Infrastructure.Tests.Translators
{
    using System.Collections.Generic;

    using AutoMapper;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Entities.Zoo;
    using Infrastructure.Translators;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class BearDetailsTranslatorShould
    {
        private IMapper mapper;

        [SetUp]
        public void BeforeEach()
        {
            var configuration = new MapperConfiguration(config => config.AddProfile(new BearDetailsTranslator()));
            this.mapper = configuration.CreateMapper();
        }
        
        [Test]
        public void MapAnimalEats()
        {
            const string expectedFoodName = "Honey";
            var animal = Builder<Animal>.CreateNew()
                                        .With(
                                            b => b.AnimalEats = new List<AnimalEat>
                                                                    {
                                                                        new AnimalEat
                                                                            { Food = new Food { Name = expectedFoodName } }
                                                                    })
                                        .Build();
            var bear = this.mapper.Map<BearDetails>(animal);
            
            bear.Foods.Should().ContainSingle(food => food == expectedFoodName);
        }
        
        [Test]
        public void MapName()
        {
            const string expectedName = "Winnie";
            var animal = Builder<Animal>.CreateNew()
                                      .With(b => b.Name = expectedName)
                                      .Build();
            var bear = this.mapper.Map<BearDetails>(animal);

            bear.Name.Should().Be(expectedName);
        }
        
        [Test]
        public void MapLegs()
        {
            const int expectedLegs = 3;
            var animal = Builder<Animal>.CreateNew()
                                        .With(b => b.Legs = expectedLegs)
                                        .Build();
            var bear = this.mapper.Map<BearDetails>(animal);

            bear.Legs.Should().Be(expectedLegs);
        }
        
        [Test]
        public void MapId()
        {
            const int expectedId = 6;
            var animal = Builder<Animal>.CreateNew()
                                        .With(b => b.Id = expectedId)
                                        .Build();
            var bear = this.mapper.Map<BearDetails>(animal);

            bear.Id.Should().Be(expectedId);
        }
    }
}