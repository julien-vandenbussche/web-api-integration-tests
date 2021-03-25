namespace Zoo.Application.Queries
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Park.BooksAggregate;
    using Park.BooksAggregate.Models;

    public interface IGetBooksAboutQuery
    {
        Task GetAsync(string animal, FoundCallback<IImmutableList<Book>> found, NotFoundCallback notFound);
    }
    
    internal sealed class GetBooksAboutQuery : IGetBooksAboutQuery
    {
        private readonly IBookAdapter bookAdapter;

        public GetBooksAboutQuery(IBookAdapter bookAdapter)
        {
            this.bookAdapter = bookAdapter;
        }

        public async Task GetAsync(string animal, FoundCallback<IImmutableList<Book>> found, NotFoundCallback notFound)
        {
            var books = await this.bookAdapter.GetBooksAsync(animal);
            if (!books.Any())
            {
                notFound();
                return;
            }

            found(books);
        }
    }
}