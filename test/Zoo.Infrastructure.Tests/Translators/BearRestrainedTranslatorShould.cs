namespace Zoo.Infrastructure.Tests.Translators
{
    using AutoMapper;

    using Entities.Zoo;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Translators;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class BearRestrainedTranslatorShould
    {
        private IMapper mapper;

        [SetUp]
        public void BeforeEach()
        {
            var configuration = new MapperConfiguration(config => config.AddProfile(new BearRestrainedTranslator()));
            this.mapper = configuration.CreateMapper();
        }
        
        [Test]
        public void MapName()
        {
            const string expectedName = "Winnie";
            var animal = Builder<Animal>.CreateNew()
                                      .With(b => b.Name = expectedName)
                                      .Build();
            var bear = this.mapper.Map<BearRestrained>(animal);

            bear.Name.Should().Be(expectedName);
        }
        
        [Test]
        public void MapId()
        {
            const int expectedId = 6;
            var animal = Builder<Animal>.CreateNew()
                                        .With(b => b.Id = expectedId)
                                        .Build();
            var bear = this.mapper.Map<BearRestrained>(animal);

            bear.Id.Should().Be(expectedId);
        }
    }
}