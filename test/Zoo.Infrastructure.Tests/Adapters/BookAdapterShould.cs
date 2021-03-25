namespace Zoo.Infrastructure.Tests.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using AxaFrance.Extensions.ServiceModel;

    using BookService;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Humanizer;

    using Infrastructure.Adapters;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class BookAdapterShould
    {
        private Mock<BookServiceChannel> mockedBookServiceChannel;

        private BookAdapter adapter;

        private Mock<IMapper> mockedMapper;

        [SetUp]
        public void BeforeEach()
        {
            var mockedServiceWrapper = new Mock<IServiceClientWrapper<BookServiceChannel>>();
            this.mockedBookServiceChannel = new Mock<BookServiceChannel>();
            mockedServiceWrapper.Setup(wrapper => wrapper.Channel)
                                .Returns(this.mockedBookServiceChannel.Object);
            this.mockedMapper = new Mock<IMapper>();
            this.adapter = new BookAdapter(mockedServiceWrapper.Object, this.mockedMapper.Object);
        }

        [TestCase("bear")]
        [TestCase("bears")]
        public async Task ReturnBooksThatContainTheExpectedWord(string searchedWord)
        {
            var expectedBooks = Builder<Park.BooksAggregate.Models.Book>.CreateListOfSize(2).Build().ToList();
            var random = new Random();
            var books = Builder<Book>.CreateListOfSize(10)
                                     .Section(3, 7)
                                     .With(book => book.Title = RandomTitle(random, searchedWord.ToUpperInvariant()))
                                     .Build();
            var filteredBooks = books.Where(book => book.Title.ToLowerInvariant().Contains(searchedWord));
            this.mockedMapper.Setup(mapper => mapper.Map<List<Park.BooksAggregate.Models.Book>>(filteredBooks))
                .Returns(expectedBooks);
            this.mockedBookServiceChannel
                .Setup(channel => channel.GetAllBooksAsync(It.IsNotNull<GetAllBooksRequest>()))
                .ReturnsAsync(
                    new GetAllBooksResponse
                        {
                            Book = books.ToArray()
                        });

            var actual = await this.adapter.GetBooksAsync(searchedWord.Pluralize());

            actual.Should().BeEquivalentTo(expectedBooks);
        }

        private static string RandomTitle(Random rand, string bear)
        {
            var words = Faker.Lorem.Words(10).ToList();
            words.Insert(rand.Next(9), bear);
            return string.Join(" ", words);
        }
    }
}