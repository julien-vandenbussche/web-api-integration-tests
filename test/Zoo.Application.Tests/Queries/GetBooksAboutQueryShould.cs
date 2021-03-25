namespace Zoo.Application.Tests.Queries
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Application.Queries;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Park.BooksAggregate;
    using Park.BooksAggregate.Models;

    [TestFixture]
    public class GetBooksAboutQueryShould
    {
        [Test]
        public async Task CallFoundCallbackWhenCallGetAsyncAndHaveAnyBooks()
        {
            var expectedBooks = Builder<Book>.CreateListOfSize(12).Build().ToImmutableList();
            var mockedFoundCallback = new Mock<FoundCallback<IImmutableList<Book>>>();
            var mockedBookAdapter = new Mock<IBookAdapter>();
            const string searchedWord = "wathever";
            mockedBookAdapter.Setup(adapter => adapter.GetBooksAsync(searchedWord))
                             .ReturnsAsync(expectedBooks);
            var query = new GetBooksAboutQuery(mockedBookAdapter.Object);

            await query.GetAsync(searchedWord, mockedFoundCallback.Object, null);
            
            mockedFoundCallback.Verify(found => found(expectedBooks), Times.Once);
        }
        
        [Test]
        public async Task CallNotFoundCallbackWhenCallGetAsyncAndNotHaveAnyBooks()
        {
            var mockedNotFoundCallback = new Mock<NotFoundCallback>();
            var mockedBookAdapter = new Mock<IBookAdapter>();
            const string searchedWord = "wathever";
            mockedBookAdapter.Setup(adapter => adapter.GetBooksAsync(searchedWord))
                             .ReturnsAsync(Enumerable.Empty<Book>().ToImmutableList());
            var query = new GetBooksAboutQuery(mockedBookAdapter.Object);

            await query.GetAsync(searchedWord, null, mockedNotFoundCallback.Object);
            
            mockedNotFoundCallback.Verify(notFound => notFound(), Times.Once);
        }
    }
}