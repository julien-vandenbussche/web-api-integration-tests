namespace Zoo.Infrastructure.Tests.Translators
{
    using System.Collections.Generic;

    using Administration.AnimalsRegistrationAggregate.Models;

    using AutoMapper;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Entities.Zoo;
    using Infrastructure.Translators;

    using NUnit.Framework;

    [TestFixture]
    public class BearCreatingTranslatorShould
    {
        private IMapper mapper;

        [SetUp]
        public void BeforeEach()
        {
            var configuration = new MapperConfiguration(config => config.AddProfile(new BearCreatingTranslator()));
            this.mapper = configuration.CreateMapper();
        }
        
        [Test]
        public void MapFamilyId()
        {
            const int expectedFamilyId = 3;
            var bear = Builder<BearCreating>.CreateNew()
                                            .Build();
            var animal = this.mapper.Map<Animal>(bear);
            
            animal.FamilyId.Should().Be(expectedFamilyId);
        }
        
        [Test]
        public void MapAnimalEats()
        {
            const int expectedFoodId = 4;
            var bear = Builder<BearCreating>.CreateNew()
                                            .With(b => b.Foods = new List<int>{expectedFoodId})
                                            .Build();
            var animal = this.mapper.Map<Animal>(bear);
            
            animal.AnimalEats.Should().ContainSingle(ae => ae.FoodId == expectedFoodId);
        }
        
        [Test]
        public void MapName()
        {
            const string expectedName = "Winnie";
            var bear = Builder<BearCreating>.CreateNew()
                                            .With(b => b.Name = expectedName)
                                            .Build();
            var animal = this.mapper.Map<Animal>(bear);

            animal.Name.Should().Be(expectedName);
        }
        
        [Test]
        public void MapLegs()
        {
            const int expectedLegs = 3;
            var bear = Builder<BearCreating>.CreateNew()
                                            .With(b => b.Legs = expectedLegs)
                                            .Build();
            var animal = this.mapper.Map<Animal>(bear);

            animal.Legs.Should().Be(expectedLegs);
        }
    }
}