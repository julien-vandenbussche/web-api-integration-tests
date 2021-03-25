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

    using Park.BooksAggregate.Models;

    public class BookTranslatorShould
    {
        private IMapper mapper;

        [SetUp]
        public void BeforeEach()
        {
            var configuration = new MapperConfiguration(config => config.AddProfile(new BookTranslator()));
            this.mapper = configuration.CreateMapper();
        }

        [Test]
        public void MapId()
        {
            const string expectedId = "X456B";
            var book = Builder<Book>.CreateNew()
                                    .With(b => b.Id = expectedId)
                                    .Build();
            var actual = this.mapper.Map<BookService.Book>(book);

            actual.ID.Should().Be(expectedId);
        }

        [Test]
        public void MapAuthor()
        {
            const string expectedAuthor = "Michel Klein";
            var book = Builder<Book>.CreateNew()
                                    .With(b => b.Author = expectedAuthor)
                                    .Build();
            var actual = this.mapper.Map<BookService.Book>(book);

            actual.Author.Should().Be(expectedAuthor);
        }

        [Test]
        public void MapTitle()
        {
            const string expectedTitle = "The three little pigs";
            var book = Builder<Book>.CreateNew()
                                    .With(b => b.Title = expectedTitle)
                                    .Build();
            var actual = this.mapper.Map<BookService.Book>(book);

            actual.Title.Should().Be(expectedTitle);
        }
    }
}