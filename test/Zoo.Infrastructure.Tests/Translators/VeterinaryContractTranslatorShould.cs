namespace Zoo.Infrastructure.Tests.Translators
{
    using System;

    using AutoMapper;

    using Contracts.Veterinary.Models;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infirmary.VeterinaryAggregate.Models;

    using Infrastructure.Translators;

    using NUnit.Framework;

    public class VeterinaryContractTranslatorShould
    {
        private IMapper mapper;

        [SetUp]
        public void BeforeEach()
        {
            var configuration = new MapperConfiguration(config => config.AddProfile(new VeterinaryContractTranslator()));
            this.mapper = configuration.CreateMapper();
        }

        [Test]
        public void MapName()
        {
            const string expectedName = "Michel Klein";
            var veterinary = Builder<Veterinary>.CreateNew()
                                            .With(b => b.Name = expectedName)
                                            .Build();
            var veterinaryContact = this.mapper.Map<VeterinaryContact>(veterinary);

            veterinaryContact.Name.Should().Be(expectedName);
        }

        [Test]
        public void MapAddress()
        {
            var veterinary = Builder<Veterinary>.CreateNew()
                                                .With(b => b.Address = "26 rue de la grotte")
                                                .With(b => b.PostalCode = "59200")
                                                .With(b => b.City = "Tourcoing")
                                                .With(b => b.Country = "France")
                                                .Build();
            var newLine = Environment.NewLine;
            var expectedAddress = $"{veterinary.Address}{newLine}{veterinary.PostalCode} {veterinary.City}{newLine}{veterinary.Country}";
            var veterinaryContact = this.mapper.Map<VeterinaryContact>(veterinary);

            veterinaryContact.Address.Should().Be(expectedAddress);
        }

        [Test]
        public void MapPhone()
        {
            const string expectedPhone = "0908070605";
            var veterinary = Builder<Veterinary>.CreateNew()
                                                .With(b => b.Phone = expectedPhone)
                                                .Build();
            var veterinaryContact = this.mapper.Map<VeterinaryContact>(veterinary);

            veterinaryContact.Phone.Should().Be(expectedPhone);
        }

        [Test]
        public void MapEMail()
        {
            const string expectedEMail = "toto@test.com";
            var veterinary = Builder<Veterinary>.CreateNew()
                                                .With(b => b.EMail = expectedEMail)
                                                .Build();
            var veterinaryContact = this.mapper.Map<VeterinaryContact>(veterinary);

            veterinaryContact.EMail.Should().Be(expectedEMail);
        }

        [Test]
        public void MapWebsite()
        {
            const string expectedWebsite = "http://www.toto.com";
            var veterinary = Builder<Veterinary>.CreateNew()
                                                .With(b => b.Website = expectedWebsite)
                                                .Build();
            var veterinaryContact = this.mapper.Map<VeterinaryContact>(veterinary);

            veterinaryContact.Website.Should().Be(expectedWebsite);
        }
    }
}